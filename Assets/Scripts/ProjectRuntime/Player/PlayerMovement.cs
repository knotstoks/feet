using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectRuntime.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [field: SerializeField, Header("Scene References")]
        private CharacterController CharacterController { get; set; }

        [field: SerializeField, Header("Movement Parameters")]
        private float MovementSpeed { get; set; }

        [field: SerializeField]
        private float JumpHeight { get; set; }

        [field: SerializeField, Header("Camera Settings")]
        private CinemachineVirtualCamera PlayerCamera { get; set; }

        [field: SerializeField]
        private float HorizontalSensitivity { get; set; }

        [field: SerializeField]
        private float VerticalSensitivity { get; set; }

        private PlayerInput _playerInput;

        private Vector2 _currentMovementInput;
        private Vector3 _currentMovement;
        private bool _isGrounded;
        private float _gravity = -9.81f;

        private float _xRotation;

        private void Awake()
        {
            this._playerInput = new PlayerInput();

            this._playerInput.CharacterControls.Move.started += context => this.OnMovementInput(context);
            this._playerInput.CharacterControls.Move.canceled += context => this.OnMovementInput(context);
            this._playerInput.CharacterControls.Move.performed += context => this.OnMovementInput(context);
            this._playerInput.CharacterControls.Jump.performed += context => this.OnJump(context);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            this._isGrounded = this.CharacterController.isGrounded;

            this.ProcessMove();
        }

        private void LateUpdate()
        {
            this.OnLook(this._playerInput.CharacterControls.Look.ReadValue<Vector2>());
        }

        private void OnEnable()
        {
            this._playerInput.CharacterControls.Enable();
        }

        private void OnDisable()
        {
            this._playerInput.CharacterControls.Disable();
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            this._currentMovementInput = context.ReadValue<Vector2>();
            this._currentMovement.x = this._currentMovementInput.x;
            this._currentMovement.z = this._currentMovementInput.y;
        }

        private void ProcessMove()
        {
            this.CharacterController.Move(this.MovementSpeed * Time.deltaTime * this.transform.TransformDirection(this._currentMovement));
            this._currentMovement.y += this._gravity * Time.deltaTime;
            if (this._isGrounded && this._currentMovement.y < 0f)
            {
                this._currentMovement.y = -2f;
            }
            this.CharacterController.Move(this._currentMovement * Time.deltaTime);
        }

        private void OnJump(InputAction.CallbackContext _)
        {
            if (this._isGrounded)
            {
                this._currentMovement.y = Mathf.Sqrt(this.JumpHeight * -3f * this._gravity);
            }
        }

        private void OnLook(Vector2 input)
        {
            var mouseX = input.x;
            var mouseY = input.y;

            this._xRotation -= mouseY * Time.deltaTime * this.VerticalSensitivity;
            this._xRotation = Mathf.Clamp(this._xRotation, -80f, 80f);
            this.PlayerCamera.transform.localRotation = Quaternion.Euler(this._xRotation, 0f, 0f);
            this.transform.Rotate((mouseX * Time.deltaTime) * this.HorizontalSensitivity * Vector3.up);
        }
    }
}