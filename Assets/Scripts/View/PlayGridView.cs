using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class PlayGridView : MonoBehaviour
    {
        [SerializeField]
        public Transform playGridSpaceParent;

        [SerializeField]
        private GameObject playGridSpacePrefab;

        [SerializeField]
        private GameObject playGridItemPrefab;

        private Dictionary<GridSpace, GameObject> gridSpaceMapping = new Dictionary<GridSpace, GameObject>();


        private Dictionary<GridItem, GameObject> gridItemMapping = new Dictionary<GridItem, GameObject>();

        void Start()
        {
            PlayGridController.Instance.RegisterPlayGridCreatedCallback(OnPlayGridCreated);
        }

        private void OnPlayGridCreated(PlayGrid grid)
        {
            grid.RegisterGridSpaceCreated(OnGridSpaceCreated);
            grid.RegisterGridItemCreated(OnGridItemCreated);
            grid.RegisterGridItemRemoved(OnGridItemRemoved);
        }

        private void OnGridItemCreated(GridItem item)
        {
            GameObject go = Instantiate(playGridItemPrefab, playGridSpaceParent);
            go.transform.position = playGridSpaceParent.position + new Vector3(item.x, item.y, 0);

            go.GetComponent<GridItemsVisuals>().SetUpVisuals(GetComponent<HandView>().itemSprites[(int)item.type]);

            gridItemMapping.Add(item, go);
        }

        private void OnGridItemRemoved(GridItem item)
        {
            if (gridItemMapping.Remove(item, out GameObject go))
            {
                Destroy(go);
            }
        }

        private void OnGridSpaceCreated(GridSpace space)
        {
            GameObject go = Instantiate(playGridSpacePrefab, playGridSpaceParent);

            go.transform.position = playGridSpaceParent.position + new Vector3(space.x, space.y, 0);

            gridSpaceMapping.Add(space, go);
        }

        public GridSpace GetGridSpace(float x, float y)
        {
            const float size = 1f;

            foreach (var item in gridSpaceMapping)
            {
                if (playGridSpaceParent.position.x + item.Key.x + size * 0.5f > x &&
                    playGridSpaceParent.position.x + item.Key.x - size * 0.5f < x &&
                    playGridSpaceParent.position.y + item.Key.y + size * 0.5f > y &&
                    playGridSpaceParent.position.y + item.Key.y - size * 0.5f < y)
                {
                    return item.Key;
                }
            }

            return null;
        }
    }
}
