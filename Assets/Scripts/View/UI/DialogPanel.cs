using JetBrains.Annotations;
using System;
using UnityEngine;
using static SnaroDev.LD60.DialogPanel;

namespace SnaroDev.LD60
{
    public class DialogPanel : Panel
    {
        [SerializeField]
        private TMPro.TMP_Text dialogBody;

        private DialogPanelOpenInfo dialogPanelOpenInfo;

        private int currentDialogIndex;

        public override void Open(PanelOpenInfo panelOpenInfo)
        {
            InputController.Instance.cbInputConfim += OnInputConfirm;

            dialogPanelOpenInfo = (DialogPanelOpenInfo)panelOpenInfo;

            dialogBody.text = dialogPanelOpenInfo.items[currentDialogIndex].body;
        }

        private void OnDestroy()
        {
            InputController.Instance.cbInputConfim -= OnInputConfirm;
        }

        private void OnInputConfirm()
        {
            currentDialogIndex++;
            if (currentDialogIndex >= dialogPanelOpenInfo.items.Length)
            {
                dialogPanelOpenInfo.cbDialogCompleted();
                UIController.Instance.CloseCurrentPanel();

                return;
            }
            
            
            dialogBody.text = dialogPanelOpenInfo.items[currentDialogIndex].body;
        }

        public class DialogPanelOpenInfo : PanelOpenInfo
        {
            public Action cbDialogCompleted;

            public DialogTextItem[] items;
        }

        public class DialogTextItem
        {
            public string body;

            public DialogTextItem(string body)
            {
                this.body = body;
            }
        }
    }
}
