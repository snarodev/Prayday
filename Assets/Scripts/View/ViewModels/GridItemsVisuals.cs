using System;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class GridItemsVisuals : MonoBehaviour
    {
        public int handOrder = 0;

        [SerializeField]
        private SpriteRenderer itemSr;

        public void SetUpVisuals(Sprite sprite)
        {
            itemSr.sprite = sprite;
        }
    }
}
