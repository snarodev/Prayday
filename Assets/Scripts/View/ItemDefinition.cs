using System;

namespace SnaroDev.LD60
{
    public class ItemDefinition
    {
        public GridItemType type;

        public string name;

        public string description;

        public int gain;

        public Action<GridItem> activationEffect;

        public ItemDefinition(GridItemType type, string name, string description, int gain, Action<GridItem> activationEffect)
        {
            this.type = type;
            this.name = name;
            this.description = description;
            this.gain = gain;
            this.activationEffect = activationEffect;
        }
    }
}
