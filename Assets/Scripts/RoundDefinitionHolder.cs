using UnityEngine;
using System.Collections.Generic;

namespace SnaroDev.LD60
{
    public static class RoundDefinitionHolder
    {
        public static List<RoundDefinition> roundDefinitions = new List<RoundDefinition>();

        static RoundDefinitionHolder()
        {
            roundDefinitions.Clear();

            roundDefinitions.Add(new RoundDefinition(0, GridItemType.Vase, 4, 4, new string[]
            {
                "Hey! You there! Aren't you the new temple apprentice?",
                "You're late! The morning worshippers are already docking by the Nile!",
                "Did you bake in the desert sun too long? Take that Vase and place it on the sacred pedestal!",
                "When the worshippers pray, their devotion flows across the altar from all four sides. Now hurry up and let them pray!",
            }));

            roundDefinitions.Add(new RoundDefinition(1, GridItemType.Shabti, 8, 12, new string[]
            {
                "Hmph. Yesterday's prayers actually reached the heavens. Colour me surprised.",
                "Word is spreading down the river, though more villagers showed up today, and the gods expect a grander offering.",
                "Here, use this Shabti figurine. It channels twice as much devotion as a plain clay vase.",
            }));

            roundDefinitions.Add(new RoundDefinition(2, GridItemType.Bird, 12, 18, new string[]
            {
                "Look at the riverbank! Merchants and scribes are lining up outside the temple gates.",
                "We're running out of altar space, so you'll need to be clever about where you place the relics.",
                "The Sacred Bird blesses whatever relic sits directly above it, doubling its points every time a prayer passes through!",
            }));

            roundDefinitions.Add(new RoundDefinition(3, GridItemType.Vase, 16, 24, new string[]
            {
                "The merchants are singing our praises all the way to Memphis!",
                "Take another Vase. Try placing it where our Sacred Bird can bless it, or along a busy prayer path.",
            }));

            roundDefinitions.Add(new RoundDefinition(4, GridItemType.Ankh, 20, 30, new string[]
            {
                "Careful with this next relic, apprentice! Don't drop it!",
                "The Ankh radiates raw divine energy. It grants strong favor, but it will shatter any relics sitting immediately to its left and right.",
                "Keep it away from our best offerings, or use it to clear out old junk!",
            }));

            roundDefinitions.Add(new RoundDefinition(5, GridItemType.Shabti, 24, 36, new string[]
            {
                "Whew, the altar survived the Ankh's power! And look, noble families are arriving on their river barges.",
                "Here is another Shabti figurine to bolster the pedestal before the afternoon prayers begin.",
            }));

            roundDefinitions.Add(new RoundDefinition(6, GridItemType.Papyrus, 32, 48, new string[]
            {
                "A royal messenger just arrived! The Pharaoh's advisor is watching today's ceremony!",
                "Take this Papyrus scroll. It offers no points on its own, but every prayer that touches it doubles our entire accumulated score!",
                "Try to let the prayers hit our other relics first before they reach the scroll!",
            }));

            roundDefinitions.Add(new RoundDefinition(7, GridItemType.Bird, 40, 60, new string[]
            {
                "The Pharaoh's advisor was impressed! He's sending word back to the royal palace!",
                "Use this Sacred Bird to amplify our strongest relics even further.",
            }));

            roundDefinitions.Add(new RoundDefinition(8, GridItemType.Wave, 48, 72, new string[]
            {
                "The sky is darkening and the river is swelling... the gods of the Nile are testing us!",
                "This Wave tablet carries the fury of the flood. It grants solid favor, but washes away three random relics on the pedestal!",
                "Only place it if the pedestal is getting crowded. Or if you're feeling lucky!",
            }));

            roundDefinitions.Add(new RoundDefinition(9, GridItemType.Papyrus, 60, 90, new string[]
            {
                "The storm has passed, and the Nile is gleaming in the sun!",
                "Tomorrow the Pharaoh himself arrives. Use this sacred Papyrus scroll to prepare the altar for his arrival!",
            }));

            roundDefinitions.Add(new RoundDefinition(10, GridItemType.Shabti, 80, 120, new string[]
            {
                "The Pharaoh's golden barge is anchored outside! The whole kingdom is praying at our temple now!",
                "The gods demand nothing short of a miracle today. Show them what you've learned, apprentice!",
            }));
        }
    }
}
