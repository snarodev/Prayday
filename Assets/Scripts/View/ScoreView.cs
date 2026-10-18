using System;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text currentScoreText;
        [SerializeField]
        private TMPro.TMP_Text targetScoreText;
        [SerializeField]
        private TMPro.TMP_Text currentRoundNumberText;
        [SerializeField]
        private TMPro.TMP_Text remainingLives;

        private void Start()
        {
            ScoreController.Instance.RegisterOnScoreCurrentChanged(OnScoreChanged);
            ScoreController.Instance.RegisterOnScoreTargetChanged(OnScoreTargetChanged);
            ScoreController.Instance.RegisterOnRoundNumberChanged(OnRoundNumber);
            ScoreController.Instance.RegisterOnRemainingLivesChanged(OnRemainingLivesChanged);
        }

        private void OnRoundNumber(int newRoundNumber)
        {
            currentRoundNumberText.text = "Round: " + newRoundNumber.ToString();
        }

        private void OnScoreTargetChanged(int newTarget)
        {
            targetScoreText.text = "Target: " + newTarget.ToString();
        }

        private void OnScoreChanged(int newScore)
        {
            currentScoreText.text = "Score: " + newScore.ToString();
        }

        private void OnRemainingLivesChanged(int newRemainingLives)
        {
            remainingLives.text = "Remaining Lives: " + newRemainingLives.ToString();
        }
    }
}
