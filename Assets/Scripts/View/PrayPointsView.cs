using System;
using UnityEngine;
using System.Collections.Generic;

namespace SnaroDev.LD60
{
    public class PrayPointsView : MonoBehaviour
    {
        [SerializeField]
        private Transform prayPointParent;

        [SerializeField]
        private GameObject prayPointPrefab;

        private float animationProgress = 0;

        private Dictionary<PrayPoint, PrayPointVisuals> prayPointVisualsMapping = new Dictionary<PrayPoint, PrayPointVisuals>();

        void Start()
        {
            PlayGridController.Instance.RegisterPlayGridCreatedCallback(OnPlayGridCreated);
        }

        private void OnPlayGridCreated(PlayGrid grid)
        {
            grid.RegisterPrayPointCreated(OnPrayPointCreated);
            grid.RegisterPrayPointEvent(OnPrayPointEvent);
            grid.RegisterPrayPointRemoved(OnPrayPointRemoved);
        }

        private void OnPrayPointEvent(PrayPointEvent prayPointEvent)
        {
            PrayPointVisuals go = prayPointVisualsMapping[prayPointEvent.prayPoint];

            go.SetTargetPos (prayPointEvent.prayPoint);
        }

        private void Update()
        {
            foreach (var item in prayPointVisualsMapping)
            {
                item.Value.UpdateVisuals(animationProgress);
            }

            animationProgress += Time.deltaTime;

            if (animationProgress >= 1)
            {
                PlayGridController.Instance.ContinueEval();
                animationProgress = 0;
            }
        }

        private void OnPrayPointCreated(PrayPoint prayPoint)
        {
            GameObject go = Instantiate(prayPointPrefab, prayPointParent);

            go.transform.position = prayPointParent.position + new Vector3(prayPoint.x, prayPoint.y, 0);
            PrayPointVisuals v = go.GetComponent<PrayPointVisuals>();

            v.Init(new Vector3(prayPoint.x, prayPoint.y, 0));

            prayPointVisualsMapping.Add(prayPoint, v);
        }

        private void OnPrayPointRemoved(PrayPoint prayPoint)
        {
            PrayPointVisuals go = prayPointVisualsMapping[prayPoint];

            Destroy(go.gameObject);

            prayPointVisualsMapping.Remove (prayPoint);
        }
    }
}
