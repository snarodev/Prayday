using JetBrains.Annotations;
using System;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class Bootstrap : MonoBehaviour
    {
        private bool didCallCallbacks = false;

        private void Update()
        {
            if (!didCallCallbacks)
            {
                didCallCallbacks = true;
                UIController.Instance.OpenPanel<MainMenuPanel>(null);
                GameStateController.Instance.SetGameState(GameState.MainMenu);
            }
        }
    }

    public enum GameState
    {
        None,
        MainMenu,
        GameBegins,
        InGamePrepare,
        InGameEvaluate,
        InGamePostEvaluate,
        GameOver,
    }
}
