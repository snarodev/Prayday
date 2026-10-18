using UnityEngine;
using System.Collections.Generic;
using System;

namespace SnaroDev.LD60
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Panel[] panels;

        [SerializeField]
        private Transform panelParent;

        public static UIController Instance  { get; internal set; }

        private Stack<Panel> panelStack = new Stack<Panel>();

        private void Awake()
        {
            Instance = this;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
        }
        void Start()
        {

        }

        public void OpenPanel<T>(PanelOpenInfo panelOpenInfo) where T : Panel
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].GetType() == typeof(T))
                {
                    GameObject go = Instantiate(panels[i].gameObject, panelParent);

                    Panel p = go.GetComponent<Panel>();
                    p.Open(panelOpenInfo);
                    panelStack.Push(p);

                    return;
                }
            }


            
            Debug.LogError($"There is no panel for type {typeof (T)}");
        }

        public void CloseCurrentPanel()
        {
            if (panelStack.Count > 0)
            {
                var p = panelStack.Pop();

                Destroy(p.gameObject);
            }
        }
    }

    public abstract class Panel : MonoBehaviour
    {
        public abstract void Open(PanelOpenInfo panelOpenInfo);
    }

    public abstract class PanelOpenInfo
    {

    }
}
