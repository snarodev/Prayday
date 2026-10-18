using System;
using UnityEngine;
using System.Collections.Generic;

namespace SnaroDev.LD60
{
    public class PlayGridController : MonoBehaviour
    {
        [HideInInspector]
        public PlayGrid playGrid;

        [HideInInspector]
        public ItemHand itemHand;

        public static PlayGridController Instance { get; internal set; }

        private Action<PlayGrid> cbPlayGridCreated;
        private Action<ItemHand> cbHandCreated;


        [SerializeField]
        private GameObject letThemPrayButton;

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
            GameStateController.Instance.RegisterOnGameStateChanged(OnGameStateChanged);

            letThemPrayButton.SetActive(false);
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.GameBegins)
            {
                playGrid = new PlayGrid();
                itemHand = new ItemHand();
                cbPlayGridCreated?.Invoke(playGrid);
                cbHandCreated?.Invoke(itemHand);
                playGrid.InitEmptySpaces(4, 4);
            }
            else if (state == GameState.InGamePrepare)
            {
                GridItemType itemType = RoundDefinitionHolder.roundDefinitions[ScoreController.Instance.GetRoundNumber()].type;
                itemHand.GetNewItem(itemType);

                letThemPrayButton.SetActive(true);
            }
            else if (state == GameState.InGameEvaluate)
            {
                letThemPrayButton.SetActive(false);
            }
        }

        public void LetThemPrayButton()
        {
            GameStateController.Instance.SetGameState(GameState.InGameEvaluate);
            playGrid.CreatePrayPoints();
            continueEval = true;
        }

        bool continueEval = false;
        public void ContinueEval ()
        {
            continueEval = true;
        }

        private void Update()
        {
            if (GameStateController.Instance.GetGameState () == GameState.InGameEvaluate)
            {
                if (continueEval)
                {
                    bool done = playGrid.UpdatePrayPoints();
                    continueEval = false;

                    if (done)
                    {
                        GameStateController.Instance.SetGameState(GameState.InGamePostEvaluate);

                        playGrid.DestroyPrayPoints();
                    }
                    else
                    {


                    }
                }
            }
        }

        public void MoveHandItemToGrid(GridItem currentDrag, GridSpace gridSpace)
        {
            if (itemHand == null)
            {
                Debug.LogError("item hand is null");
            }

            itemHand.RemoveHandItem(currentDrag);

            playGrid.AddItemToGrid(currentDrag, gridSpace);
        }

        private void OnDestroy()
        {
            GameStateController.Instance.UnregisterOnGameStateChanged(OnGameStateChanged);
        }

        public void RegisterPlayGridCreatedCallback(Action<PlayGrid> cb)
        {
            cbPlayGridCreated += cb;
        }
        public void UnregisterPlayGridCreatedCallback(Action<PlayGrid> cb)
        {
            cbPlayGridCreated -= cb;
        }

        public void RegisterHandCreatedCallback(Action<ItemHand> cb)
        {
            cbHandCreated += cb;
        }
        public void UnregisterHandCreatedCallback(Action<ItemHand> cb)
        {
            cbHandCreated -= cb;
        }
    }
}
