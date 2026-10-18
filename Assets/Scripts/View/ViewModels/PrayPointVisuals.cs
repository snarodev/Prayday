using System;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class PrayPointVisuals : MonoBehaviour
    {
        public Vector2 currentPos;
        public Vector2 targetPos;

        public void Init(Vector3 vector3)
        {
            currentPos = vector3;
        }

        public void SetTargetPos(PrayPoint prayPoint)
        {
            currentPos = transform.localPosition;
            targetPos = new Vector2 (prayPoint.x, prayPoint.y);
        }
        public void UpdateVisuals(float animationProgress)
        {
            transform.localPosition = Vector3.Lerp(currentPos, targetPos, animationProgress);
        }
    }
}
