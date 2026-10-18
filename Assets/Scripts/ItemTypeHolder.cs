using UnityEngine;
using System.Collections.Generic;

namespace SnaroDev.LD60
{
    public static class ItemTypeHolder
    {
        public static List<ItemDefinition> itemDefinition = new List<ItemDefinition>();

        static ItemTypeHolder ()
        {
            itemDefinition.Clear();

            itemDefinition.Add(new ItemDefinition(GridItemType.Vase, "Vase", "Gives one point.", 1, (item) => { }));
            itemDefinition.Add(new ItemDefinition(GridItemType.Bird, "Bird", "The item above this one gives double points.", 1, (item) =>
            {
                GridItem aboveItem = PlayGridController.Instance.playGrid.GetItemAt(item.x, item.y + 1);
                if (aboveItem != null)
                {
                    aboveItem.pointMultiplier *= 2;
                }
            }));
            itemDefinition.Add(new ItemDefinition(GridItemType.Papyrus, "Papyrus", "Double the final amount of points", 0, (item) =>
            {
                ScoreController.Instance.AddScore(ScoreController.Instance.GetCurrentScore());
            }));
            itemDefinition.Add(new ItemDefinition(GridItemType.Shabti, "Shabti", "Gives two points", 2, (item) => { }));
            itemDefinition.Add(new ItemDefinition(GridItemType.Ankh, "Ankh", "Destroys the left and right item.", 2, (item) =>
            {
                PlayGrid grid = PlayGridController.Instance.playGrid;
                GridItem leftItem = grid.GetItemAt(item.x - 1, item.y);
                if (leftItem != null)
                {
                    grid.RemoveItemFromGrid(leftItem);
                }

                GridItem rightItem = grid.GetItemAt(item.x + 1, item.y);
                if (rightItem != null)
                {
                    grid.RemoveItemFromGrid(rightItem);
                }
            }));
            itemDefinition.Add(new ItemDefinition(GridItemType.Wave, "Wave", "Destroys three random items and itself.", 2, (item) =>
            {
                PlayGrid grid = PlayGridController.Instance.playGrid;
                List<GridItem> candidates = new List<GridItem>();
                for (int i = 0; i < grid.gridItems.Count; i++)
                {
                    if (grid.gridItems[i] != item && !grid.gridItems[i].inHand)
                    {
                        candidates.Add(grid.gridItems[i]);
                    }
                }

                int destroyCount = Mathf.Min(3, candidates.Count);
                for (int i = 0; i < destroyCount; i++)
                {
                    int randomIndex = Random.Range(0, candidates.Count);
                    grid.RemoveItemFromGrid(candidates[randomIndex]);
                    candidates.RemoveAt(randomIndex);
                }

                grid.RemoveItemFromGrid(item);
            }));
        }

        public static ItemDefinition GetItemDefinition(GridItemType type)
        {
            for (int i = 0; i < itemDefinition.Count; i++)
            {
                if (itemDefinition[i].type == type)
                {
                    return itemDefinition[i];
                }
            }
            return null;
        }
    }
}
