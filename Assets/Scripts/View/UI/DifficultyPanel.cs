using UnityEngine;

namespace SnaroDev.LD60
{
    public class DifficultyPanel : Panel
    {
        public override void Open(PanelOpenInfo panelOpenInfo)
        {

        }

        public void StartGameButton ()
        {
            ScoreController.Instance.isHardMode = false;
            UIController.Instance.CloseCurrentPanel();
            GameStateController.Instance.SetGameState(GameState.GameBegins);
        }
        public void StartGameHardButton ()
        {
            ScoreController.Instance.isHardMode = true;
            UIController.Instance.CloseCurrentPanel();
            GameStateController.Instance.SetGameState(GameState.GameBegins);
        }
    }
}
