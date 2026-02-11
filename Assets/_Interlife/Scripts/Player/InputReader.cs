using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Interlife.Player
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Interlife/Input Reader")]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        public event Action<Vector2> MoveEvent;
        public event Action JumpEvent;
        public event Action JumpCancelledEvent;
        public event Action DashEvent;
        public event Action CrouchEvent;
        public event Action InteractionEvent;

        private InputSystem_Actions inputActions;

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new InputSystem_Actions();
                inputActions.Player.SetCallbacks(this);
            }
            inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpEvent?.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                JumpCancelledEvent?.Invoke();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                DashEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                CrouchEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                InteractionEvent?.Invoke();
        }

        // Acciones no utilizadas en el Core loop de Void según GDD
        public void OnLook(InputAction.CallbackContext context) { }
        public void OnAttack(InputAction.CallbackContext context) { }
        public void OnSprint(InputAction.CallbackContext context) { }
        public void OnPrevious(InputAction.CallbackContext context) { }
        public void OnNext(InputAction.CallbackContext context) { }
    }
}
