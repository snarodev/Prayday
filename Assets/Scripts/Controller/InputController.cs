using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnaroDev.LD60
{
    public class InputController : MonoBehaviour, InputSystem_Actions.IGameActions
    {
        private InputSystem_Actions inputActions;

        public static InputController Instance;

        [HideInInspector]
        public Vector2 pointerPos;

        public Action cbInputConfim;
        public Action cbPointerDown;
        public Action cbPointerUp;

        private void Awake()
        {
            Instance = this;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
        }
        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                cbInputConfim?.Invoke();
            }

            if (context.phase == InputActionPhase.Started)
            {
                cbPointerDown?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled ||
                context.phase == InputActionPhase.Disabled)
            {
                cbPointerUp?.Invoke();
            }
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Vector2 pos = context.ReadValue<Vector2>();

                pointerPos = pos;
                
            }
        }

        private void Start()
        {
            inputActions = new InputSystem_Actions();

            inputActions.Game.SetCallbacks(this);

            inputActions.Player.Enable();

            inputActions.UI.Enable();

            inputActions.Game.Enable();
        }
    }
}
