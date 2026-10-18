using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnaroDev.LD60
{
    public class ItemHand
    {
        private List<GridItem> hand = new List<GridItem>();

        private Action<GridItem> cbHandItemAdded;
        private Action<GridItem> cbHandItemRemove;

        public void RegisterHandItemAdded(Action<GridItem> cb)
        {
            cbHandItemAdded += cb;
        }
        public void UnregisterHandItemAdded(Action<GridItem> cb)
        {
            cbHandItemAdded -= cb;
        }

        public void GetNewItem(GridItemType type)
        {
            GridItem i = new GridItem()
            {
                inHand = true,
                type = type
            };

            hand.Add(i);
            cbHandItemAdded?.Invoke(i);
        }

        public void RemoveHandItem(GridItem item)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] == item)
                {
                    hand.RemoveAt(i);
                    cbHandItemRemove(item);
                }
            }
        }

        public void RegisterHandItemRemove(Action<GridItem> cb)
        {
            cbHandItemRemove += cb;
        }
        public void UnregisterHandItemRemove(Action<GridItem> cb)
        {
            cbHandItemRemove -= cb;
        }
    }
}
