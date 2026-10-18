using UnityEngine;

namespace SnaroDev.LD60
{
    public class RoundDefinition
    {
        public int roundNumber;

        public GridItemType type;

        public int targetPoints;

        public int targetPointsHard;

        public string[] priorDialogs;

        public RoundDefinition(int roundNumber, GridItemType type, int targetPoints, int targetPointsHard, string[] priorDialogs)
        {
            this.roundNumber = roundNumber;
            this.type = type;
            this.targetPoints = targetPoints;
            this.targetPointsHard = targetPointsHard;
            this.priorDialogs = priorDialogs;
        }
    }
}
