using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace SnaroDev.LD60
{
    public class HandView : MonoBehaviour
    {
        [SerializeField]
        public Transform handParent;

        [SerializeField]
        private GameObject itemPrefab;

        private Dictionary<GridItem, GridItemsVisuals> handItems = new Dictionary<GridItem, GridItemsVisuals>();

        private const int handItemWidth = 2;

        private bool pointerDown = false;

        private bool isDragging = false;

        private PlayGridView playGridView;


        private GridItem currentDrag;
        private GridSpace currentDraggedOverSpace;

        [SerializeField]
        public Sprite[] itemSprites;


        
        [SerializeField]
        private GameObject detailPanel;

        [SerializeField]
        private TMPro.TMP_Text itemNameText;
        [SerializeField]
        private TMPro.TMP_Text itemDescriptionText;

        private void Start()
        {
            PlayGridController.Instance.RegisterHandCreatedCallback(OnHandCreated);

            playGridView = GetComponent<PlayGridView>();

            currentDrag = null;
            currentDraggedOverSpace = null;

            if (detailPanel != null)
            {
                detailPanel.SetActive(false);
            }

            InputController.Instance.cbPointerDown += OnPointerDown;
            InputController.Instance.cbPointerUp += OnPointerUp;
        }

        private void OnDestroy()
        {
            InputController.Instance.cbPointerDown -= OnPointerDown;
            InputController.Instance.cbPointerUp -= OnPointerUp;
        }

        private void OnPointerDown()
        {
            pointerDown = true;
        }
        private void OnPointerUp()
        {
            if (currentDraggedOverSpace != null && currentDrag != null)
            {
                currentDrag.inHand = false;

                PlayGridController.Instance.MoveHandItemToGrid(currentDrag, currentDraggedOverSpace);
            }

            currentDraggedOverSpace = null;
            currentDrag = null;
        }



        private void OnHandCreated(ItemHand hand)
        {
            hand.RegisterHandItemAdded(OnHandItemCreated);
            hand.RegisterHandItemRemove(OnHandItemRemoved);
        }

        private void OnHandItemRemoved(GridItem item)
        {
            if (handItems.Remove(item, out var v))
            {
                Destroy(v.gameObject);
            }
        }

        private void OnHandItemCreated(GridItem item)
        {
            GameObject go = Instantiate(itemPrefab, handParent);

            var v = go.GetComponent<GridItemsVisuals>();
            v.handOrder = GetNextFreeIndex();
            v.SetUpVisuals(itemSprites[(int)item.type]);

            handItems.Add(item, v);
        }

        private int GetNextFreeIndex()
        {
            for (int t = 0; t < 10; t++)
            {
                bool found = false;
                foreach (var item in handItems)
                {
                    if (item.Value.handOrder == t)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return t;
                }
            }

            return 10;
        }


        private void Update()
        {
            Camera cam = Camera.main;

            var worldPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 0));
            worldPoint.z = 0;

            GridItem hoveredItem = null;

            foreach (var item in handItems)
            {
                Vector2 targetScale = Vector2.one * 2;

                Vector2 targetPos = worldPoint + new Vector3(item.Value.handOrder * handItemWidth, 0, 0);
                Vector2 itemPos = item.Value.transform.position;

                Vector2 pointerPosScreen = InputController.Instance.pointerPos;

                var pointerPosWorld = cam.ScreenToWorldPoint(pointerPosScreen);


                if (currentDrag == null)
                {
                    if (itemPos.x + handItemWidth * 0.5f > pointerPosWorld.x &&
                        itemPos.x - handItemWidth * 0.5f < pointerPosWorld.x &&
                        itemPos.y + handItemWidth * 0.5f > pointerPosWorld.y &&
                        itemPos.y - handItemWidth * 0.5f < pointerPosWorld.y)
                    {
                        // Pointer is over item
                        hoveredItem = item.Key;

                        targetPos.y += 0.3f;

                        if (pointerDown)
                        {
                            //currentDrag = item.Value;
                            currentDrag = item.Key;

                            targetPos = pointerPosWorld;
                        }
                    }
                }
                else if (currentDrag == item.Key)
                {
                    targetPos = pointerPosWorld;

                    currentDraggedOverSpace = playGridView.GetGridSpace(targetPos.x, targetPos.y);

                    if (currentDraggedOverSpace != null)
                    {
                        var draggedOverSpaceItem = PlayGridController.Instance.playGrid.GetItemAt (currentDraggedOverSpace.x, currentDraggedOverSpace.y);

                        if (draggedOverSpaceItem == null)
                        {

                            targetPos = (Vector2)playGridView.playGridSpaceParent.position + new Vector2Int(currentDraggedOverSpace.x, currentDraggedOverSpace.y);

                            targetScale = Vector2.one;
                        }
                    }
                }

                item.Value.transform.position = Vector3.Lerp(item.Value.transform.position, targetPos, 0.1f);
                item.Value.transform.localScale = Vector3.Lerp(item.Value.transform.localScale, targetScale, 0.1f);
            }

            if (detailPanel != null)
            {
                if (hoveredItem != null)
                {
                    ItemDefinition def = ItemTypeHolder.GetItemDefinition(hoveredItem.type);
                    if (def != null)
                    {
                        itemNameText.text = def.name;
                        itemDescriptionText.text = def.description;
                    }
                    detailPanel.SetActive(true);
                }
                else
                {
                    detailPanel.SetActive(false);
                }
            }

            pointerDown = false;
        }
    }
}
