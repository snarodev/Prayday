using UnityEngine;
using UnityEngine.SceneManagement;

namespace SnaroDev.LD60
{
    public class WinPanel : Panel
    {
        public override void Open(PanelOpenInfo panelOpenInfo)
        {

        }

        public void RestartGame ()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
