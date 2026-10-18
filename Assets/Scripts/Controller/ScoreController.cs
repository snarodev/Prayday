using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class ScoreController : MonoBehaviour
    {
        private Action<int> cbScoreCurrentChanged;
        private Action<int> cbScoreTargetChanged;
        private Action<int> cbRoundNumberChanged;
        private Action<int> cbRemainingLivesChanged;

        private int currentScore;

        private int targetScore;

        private int roundNumber;

        public static ScoreController Instance;

        public bool isHardMode;

        private int remainingLives = 5;

        [SerializeField]
        private GameObject roundWon;

        [SerializeField]
        private GameObject roundLost;

        [SerializeField]
        private GameObject hardModeIndicator;

        private void Awake()
        {
            Instance = this;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
        }
        private void Start()
        {
            if (roundWon != null)
            {
                roundWon.SetActive(false);
            }

            if (roundLost != null)
            {
                roundLost.SetActive(false);
            }

            GameStateController.Instance.RegisterOnGameStateChanged(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            GameStateController.Instance.UnregisterOnGameStateChanged(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.GameBegins)
            {
                SetRoundNumber(0);
                cbRemainingLivesChanged?.Invoke(remainingLives);

                targetScore = GetCurrentRoundTargetScore();
                cbScoreTargetChanged?.Invoke(targetScore);

                currentScore = 0;
                cbScoreCurrentChanged?.Invoke(currentScore);

                hardModeIndicator.SetActive(isHardMode);

                StartRoundWithDialog();
            }
            else if (state == GameState.InGamePostEvaluate)
            {
                StartCoroutine(EvaluateRoundResult());
            }
            else if (state == GameState.InGamePrepare)
            {
                targetScore = GetCurrentRoundTargetScore();
                cbScoreTargetChanged?.Invoke(targetScore);

                currentScore = 0;
                cbScoreCurrentChanged?.Invoke(currentScore);
            }
        }

        private int GetCurrentRoundTargetScore()
        {
            RoundDefinition roundDef = RoundDefinitionHolder.roundDefinitions[roundNumber];
            return isHardMode ? roundDef.targetPointsHard : roundDef.targetPoints;
        }

        private void StartRoundWithDialog()
        {
            string[] priorDialogs = RoundDefinitionHolder.roundDefinitions[roundNumber].priorDialogs;
            if (priorDialogs != null && priorDialogs.Length > 0)
            {
                DialogPanel.DialogTextItem[] dialogItems = new DialogPanel.DialogTextItem[priorDialogs.Length];
                for (int i = 0; i < priorDialogs.Length; i++)
                {
                    dialogItems[i] = new DialogPanel.DialogTextItem(priorDialogs[i]);
                }

                UIController.Instance.OpenPanel<DialogPanel>(new DialogPanel.DialogPanelOpenInfo()
                {
                    cbDialogCompleted = () =>
                    {
                        GameStateController.Instance.SetGameState(GameState.InGamePrepare);
                    },
                    items = dialogItems
                });
            }
            else
            {
                GameStateController.Instance.SetGameState(GameState.InGamePrepare);
            }
        }

        private IEnumerator EvaluateRoundResult()
        {
            // Either the target score was reached or not
            if (currentScore >= targetScore)
            {
                if (roundWon != null)
                {
                    roundWon.SetActive(true);
                }

                yield return new WaitForSeconds(1f);

                if (roundWon != null)
                {
                    roundWon.SetActive(false);
                }

                if (roundNumber + 1 >= RoundDefinitionHolder.roundDefinitions.Count)
                {
                    // Last round cleared
                    GameStateController.Instance.SetGameState(GameState.GameOver);
                    UIController.Instance.OpenPanel<WinPanel>(null);
                }
                else
                {
                    // Next round
                    SetRoundNumber(roundNumber + 1);
                    StartRoundWithDialog();
                }
            }
            else
            {
                // Lose a life
                remainingLives--;
                cbRemainingLivesChanged?.Invoke(remainingLives);

                if (roundLost != null)
                {
                    roundLost.SetActive(true);
                }

                yield return new WaitForSeconds(1f);

                if (roundLost != null)
                {
                    roundLost.SetActive(false);
                }

                if (remainingLives <= 0)
                {
                    // Game Over
                    GameStateController.Instance.SetGameState(GameState.GameOver);
                    UIController.Instance.OpenPanel<GameOverPanel>(null);
                }
                else
                {
                    GameStateController.Instance.SetGameState(GameState.InGamePrepare);
                }
            }
        }

        private void SetRoundNumber(int newRoundNumber)
        {
            roundNumber = newRoundNumber;
            cbRoundNumberChanged?.Invoke(roundNumber);
        }

        public int GetRoundNumber() => roundNumber;

        public int GetCurrentScore() => currentScore;

        public void AddScore(int scoreToAdd)
        {
            currentScore += scoreToAdd;

            cbScoreCurrentChanged?.Invoke(currentScore);
        }

        public void RegisterOnScoreCurrentChanged(Action<int> cb)
        {
            cbScoreCurrentChanged += cb;
        }
        public void UnregisterOnScoreChanged(Action<int> cb)
        {
            cbScoreCurrentChanged -= cb;
        }

        public void RegisterOnScoreTargetChanged(Action<int> cb)
        {
            cbScoreTargetChanged += cb;
        }
        public void UnregisterOnScoreTargetChanged(Action<int> cb)
        {
            cbScoreTargetChanged -= cb;
        }

        public void RegisterOnRoundNumberChanged(Action<int> cb)
        {
            cbRoundNumberChanged += cb;
        }
        public void UnregisterOnRoundNumberChanged(Action<int> cb)
        {
            cbRoundNumberChanged -= cb;
        }
        public void RegisterOnRemainingLivesChanged(Action<int> cb)
        {
            cbRemainingLivesChanged += cb;
        }
        public void UnregisterOnRemainingLivesChanged(Action<int> cb)
        {
            cbRemainingLivesChanged -= cb;
        }
    }
}
