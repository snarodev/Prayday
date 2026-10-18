using System;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class GameStateController : MonoBehaviour
    {
        private GameState gameState = GameState.None;

        private Action<GameState> cbGameStateChanged;

        public static GameStateController Instance { get; internal set; }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
        }
        private void Awake()
        {
            Instance = this;
        }

        public void SetGameState(GameState gameState)
        {
            if (this.gameState != gameState)
            {
                this.gameState = gameState;
                cbGameStateChanged?.Invoke(gameState);
            }
        }

        public GameState GetGameState() => gameState;

        public void RegisterOnGameStateChanged(Action<GameState> cb)
        {
            cbGameStateChanged += cb;
        }
        public void UnregisterOnGameStateChanged(Action<GameState> cb)
        {
            cbGameStateChanged -= cb;
        }
    }
}
