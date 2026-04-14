using System;
using Managers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : PlayerSystem
    {
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsAnchored = Animator.StringToHash("isAnchored");
        
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private Transform feet;
        
        public bool IsGround { get; private set; }
        private bool isAnchored;
        private bool inputEnabled = true;

        private Vector2 inputVector;
        
        private string moveInputSelection;
        private string jumpInputSelection;
        private string anchorInputSelection;
        private string retractInputSelection;
        
        public static event Action<PlayerMain.PlayerType, bool> OnAnchorChanged;
        public static event Action<PlayerMain.PlayerType> OnRetractRope;
        public static event Action<PlayerMain.PlayerType> OnStopRetractRope;
        public PlayerInput PlayerInput { get; private set; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();
            PlayerInput = GetComponent<PlayerInput>();
            //main.Rb.gravityScale = playerSettings.gravityScale;
        }

        private void OnEnable()
        {
            if (main.playerType == PlayerMain.PlayerType.PlayerOne)
            {
                moveInputSelection = "Move1";
                jumpInputSelection = "Jump1";
                anchorInputSelection = "Anchor1";
                retractInputSelection = "Retract1";
            }
            else
            {
                moveInputSelection = "Move2";
                jumpInputSelection = "Jump2";
                anchorInputSelection = "Anchor2";
                retractInputSelection = "Retract2";
            }
        
            PlayerInput.actions[moveInputSelection].performed += OnMove;
            PlayerInput.actions[moveInputSelection].canceled += OnMove;
            PlayerInput.actions[jumpInputSelection].started += OnJump;
            
            PlayerInput.actions[anchorInputSelection].started += EnableAnchor;
            PlayerInput.actions[anchorInputSelection].canceled += DisableAnchor;
            PlayerInput.actions[retractInputSelection].started += OnRetractInput;
            PlayerInput.actions[retractInputSelection].canceled += OnStopRetractInput;
        }

        private void OnDisable()
        {
            PlayerInput.actions[moveInputSelection].performed -= OnMove;
            PlayerInput.actions[moveInputSelection].canceled -= OnMove;
            PlayerInput.actions[jumpInputSelection].canceled -= OnJump;
            
            PlayerInput.actions[anchorInputSelection].started -= EnableAnchor;
            PlayerInput.actions[anchorInputSelection].canceled -= DisableAnchor;
            PlayerInput.actions[retractInputSelection].started -= OnRetractInput;
            PlayerInput.actions[retractInputSelection].canceled -= OnStopRetractInput;
        }
        void Update()
        {
            CheckGround();
            CheckRotation();
        
            main.Anim.SetBool(IsRunning, inputVector != Vector2.zero && !isAnchored && inputEnabled);
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void EnableAnchor(InputAction.CallbackContext ctx)
        {
            if (!IsGround) return;
            isAnchored = true;
            main.Anim.SetBool(IsAnchored, isAnchored);
            OnAnchorChanged?.Invoke(main.playerType, isAnchored);
            main.Rb.bodyType = RigidbodyType2D.Kinematic;
            main.Rb.linearVelocity = Vector2.zero;
        }

        private void DisableAnchor(InputAction.CallbackContext ctx)
        {
            isAnchored = false;
            main.Anim.SetBool(IsAnchored, isAnchored);
            OnAnchorChanged?.Invoke(main.playerType, isAnchored);
            main.Rb.bodyType = RigidbodyType2D.Dynamic;
        }
        
        private void OnRetractInput(InputAction.CallbackContext ctx)
        {
            if (!inputEnabled || !IsGround) return;
            OnRetractRope?.Invoke(main.playerType);
        }

        private void OnStopRetractInput(InputAction.CallbackContext ctx)
        {
            OnStopRetractRope?.Invoke(main.playerType);
        }

        public void SetInputEnabled(bool state)
        {
            inputEnabled = state;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            inputVector = ctx.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (IsGround && inputEnabled)
            {
                main.Rb.AddForce(Vector2.up * playerSettings.jumpForce, ForceMode2D.Impulse);
            }
        }

        private void CheckGround()
        {
            IsGround = Physics2D.OverlapCircle(feet.position, playerSettings.detectionRadius, playerSettings.whatIsGround);
            main.Anim.SetBool(IsJumping, !IsGround);
        }

        private void CheckRotation()
        {
            if (inputVector.x > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if (inputVector.x < 0 && transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        void UpdateMovement()
        {
            if (isAnchored || !inputEnabled) return;
            main.Rb.AddForce(inputVector.normalized * playerSettings.moveForce, ForceMode2D.Force);
        }
    
        private void OnDrawGizmos()
        {
            if (feet != null)
            {
                Gizmos.DrawSphere(feet.position, playerSettings.detectionRadius);
            }
        }
    }
}
