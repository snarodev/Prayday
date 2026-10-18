using UnityEngine;
using System.Collections.Generic;
using System;

namespace SnaroDev.LD60
{
    public class PlayGrid
    {
        public List<GridItem> gridItems = new List<GridItem>();
        public List<GridSpace> gridSpaces = new List<GridSpace>();

        public List<PrayPoint> prayPoints = new List<PrayPoint>();

        private Action<GridItem> cbGridItemCreated;
        private Action<GridItem> cbGridItemRemoved;
        private Action<GridSpace> cbGridSpaceCreated;
        private Action<PrayPoint> cbPrayPointCreated;
        private Action<PrayPoint> cbPrayPointRemoved;

        private Action<PrayPointEvent> cbPrayPointEvent;

        public void InitEmptySpaces(int height, int width)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridSpace space = new GridSpace()
                    {
                        x = x,
                        y = y
                    };

                    gridSpaces.Add(space);
                    cbGridSpaceCreated?.Invoke(space);
                }
            }
        }

        public void RegisterGridItemCreated(Action<GridItem> cb)
        {
            cbGridItemCreated += cb;
        }
        public void UnregisterGridItemCreated(Action<GridItem> cb)
        {
            cbGridItemCreated -= cb;
        }

        public void RegisterGridItemRemoved(Action<GridItem> cb)
        {
            cbGridItemRemoved += cb;
        }
        public void UnregisterGridItemRemoved(Action<GridItem> cb)
        {
            cbGridItemRemoved -= cb;
        }

        public void RegisterGridSpaceCreated(Action<GridSpace> cb)
        {
            cbGridSpaceCreated += cb;
        }
        public void UnregisterGridSpaceCreated(Action<GridSpace> cb)
        {
            cbGridSpaceCreated -= cb;
        }

        public void RegisterPrayPointCreated(Action<PrayPoint> cb)
        {
            cbPrayPointCreated += cb;
        }
        public void UnregisterPrayPointCreated(Action<PrayPoint> cb)
        {
            cbPrayPointCreated -= cb;
        }

        public void RegisterPrayPointEvent(Action<PrayPointEvent> cb)
        {
            cbPrayPointEvent += cb;
        }
        public void UnregisterPrayPointEvent(Action<PrayPointEvent> cb)
        {
            cbPrayPointEvent -= cb;
        }
        public void RegisterPrayPointRemoved(Action<PrayPoint> cb)
        {
            cbPrayPointRemoved += cb;
        }
        public void UnregisterPrayPointRemoved(Action<PrayPoint> cb)
        {
            cbPrayPointRemoved -= cb;
        }

        public void CreatePrayPoints()
        {
            for (int i = 0; i < gridItems.Count; i++)
            {
                gridItems[i].pointMultiplier = 1;
            }

            for (int i = 0; i < gridSpaces.Count; i++)
            {
                // top
                if (GetGridSpaceAt(gridSpaces[i].x, gridSpaces[i].y + 1) == null)
                {
                    CreatePrayPoint(gridSpaces[i].x, gridSpaces[i].y + 1, 0, -1);
                }
                // bottom
                if (GetGridSpaceAt(gridSpaces[i].x, gridSpaces[i].y - 1) == null)
                {
                    CreatePrayPoint(gridSpaces[i].x, gridSpaces[i].y - 1, 0, 1);
                }
                // right
                if (GetGridSpaceAt(gridSpaces[i].x + 1, gridSpaces[i].y) == null)
                {
                    CreatePrayPoint(gridSpaces[i].x + 1, gridSpaces[i].y, -1, 0);
                }
                // left
                if (GetGridSpaceAt(gridSpaces[i].x - 1, gridSpaces[i].y) == null)
                {
                    CreatePrayPoint(gridSpaces[i].x - 1, gridSpaces[i].y, 1, 0);
                }
            }
        }

        private void CreatePrayPoint(int x, int y, int movementDirX, int movementDirY)
        {
            PrayPoint p = new PrayPoint();
            p.x = x;
            p.y = y;
            p.movementDirX = movementDirX;
            p.movementDirY = movementDirY;

            prayPoints.Add(p);
            cbPrayPointCreated?.Invoke(p);
        }

        public bool UpdatePrayPoints()
        {
            bool evaluateIsDone = true;

            for (int i = 0; i < prayPoints.Count; i++)
            {
                if (GetGridSpaceAt(prayPoints[i].x, prayPoints[i].y) != null ||
                    GetGridSpaceAt(prayPoints[i].x + prayPoints[i].movementDirX, prayPoints[i].y + prayPoints[i].movementDirY) != null)
                {
                    evaluateIsDone = false;
                    prayPoints[i].x += prayPoints[i].movementDirX;
                    prayPoints[i].y += prayPoints[i].movementDirY;

                    cbPrayPointEvent?.Invoke(new PrayPointEvent()
                    {
                        type = PrayPointEventType.Move,
                        prayPoint = prayPoints[i],
                    });

                    GridItem item = GetItemAt(prayPoints[i].x, prayPoints[i].y);
                    if (item != null)
                    {
                        ActivateItem(item);

                        cbPrayPointEvent?.Invoke(new PrayPointEvent()
                        {
                            type = PrayPointEventType.Gain,
                            prayPoint = prayPoints[i],
                        });
                    }
                }
            }

            return evaluateIsDone;
        }

        private void ActivateItem(GridItem item)
        {
            ItemDefinition def = ItemTypeHolder.GetItemDefinition(item.type);
            if (def != null)
            {
                int scoreGain = def.gain * item.pointMultiplier;
                if (scoreGain != 0)
                {
                    ScoreController.Instance.AddScore(scoreGain);
                }

                def.activationEffect?.Invoke(item);
            }
        }

        private GridSpace GetGridSpaceAt(int x, int y)
        {
            for (int i = 0; i < gridSpaces.Count; i++)
            {
                if (gridSpaces[i].x == x &&
                    gridSpaces[i].y == y)
                {
                    return gridSpaces[i];
                }
            }
            return null;
        }

        public void DestroyPrayPoints()
        {
            while (prayPoints.Count > 0)
            {
                var p = prayPoints[0];

                prayPoints.RemoveAt(0);

                cbPrayPointRemoved?.Invoke(p);
            }
        }

        public void AddItemToGrid(GridItem itemToAdd, GridSpace gridSpace)
        {
            itemToAdd.x = gridSpace.x;
            itemToAdd.y = gridSpace.y;
            itemToAdd.inHand = false;

            gridItems.Add(itemToAdd);

            cbGridItemCreated?.Invoke(itemToAdd);
        }

        public void RemoveItemFromGrid(GridItem itemToRemove)
        {
            if (gridItems.Remove(itemToRemove))
            {
                cbGridItemRemoved?.Invoke(itemToRemove);
            }
        }

        public GridItem GetItemAt(int x, int y)
        {
            for (int i = 0; i < gridItems.Count; i++)
            {
                if (gridItems[i].x == x &&
                    gridItems[i].y == y &&
                    !gridItems[i].inHand)
                {
                    return gridItems[i];
                }
            }
            return null;
        }
    }

    [Serializable]
    public class GridItem
    {
        public int x;
        public int y;

        public bool inHand;

        public GridItemType type;

        public int pointMultiplier = 1;
    }

    public enum GridItemType
    {
        Vase,
        Bird,
        Ankh,
        Wave,
        Shabti,
        Papyrus,
    }

    [Serializable]
    public class GridSpace
    {
        public int x;
        public int y;
    }

    [Serializable]
    public class PrayPoint
    {
        public int x;
        public int y;

        public int movementDirX;
        public int movementDirY;
    }

    public class PrayPointEvent
    {
        public PrayPointEventType type;

        public PrayPoint prayPoint;
    }

    public enum PrayPointEventType
    {
        Move,
        Gain,
    }
}
