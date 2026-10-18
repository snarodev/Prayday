using JetBrains.Annotations;
using UnityEngine;

namespace SnaroDev.LD60
{
    public class MainMenuPanel : Panel
    {
        public override void Open(PanelOpenInfo panelOpenInfo)
        {

        }

        public void StartGameButton ()
        {
            UIController.Instance.CloseCurrentPanel();
            UIController.Instance.OpenPanel<DifficultyPanel>(null);
        }
    }
}
