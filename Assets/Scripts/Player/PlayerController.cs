
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState{
    NormalMode,
    TrashMode,
    OldManMode,
    PoliceMode
}

namespace Player
{
    // 
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class PlayerController : MonoBehaviour, IPlayer
    {
        
        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private float collisionOffset = 0.05f;
        [SerializeField] private ContactFilter2D movementFilter;
        [SerializeField] private Transform interactor;
        [SerializeField] private Transform hands;
        
        private PlayerState _playerState;
        private Vector2 _playerMovementInput;
        private Animator _playerAnimator;
        private Rigidbody2D _playerRigidbody;
        private List<RaycastHit2D> _castCollisions = new();
        
        private static readonly int Interact = Animator.StringToHash("Interact");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");

        // 
        private void Awake()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
            _playerState = PlayerState.NormalMode;
        }
        
        // 
        public void OnMovement(InputAction.CallbackContext context)
        {
            _playerMovementInput = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerAnimator.SetTrigger(Interact);
                
            }
        }
        
        // Rigidbody Movement
        public void FixedUpdate()
        {
            if (_playerMovementInput != Vector2.zero)
            {
                var success = TryMove(_playerMovementInput);
                
                if (!success)
                {
                    success = TryMove(new Vector2(_playerMovementInput.x, 0));
                
                    if (!success)
                    {
                        success = TryMove(new Vector2(0, _playerMovementInput.y));
                    }
                }
                
                _playerAnimator.SetFloat(Horizontal, _playerMovementInput.x);
                _playerAnimator.SetFloat(Vertical, _playerMovementInput.y);
                _playerAnimator.SetFloat(Speed, _playerMovementInput.sqrMagnitude);
            }
            else
            {
                _playerAnimator.SetFloat(Speed, _playerMovementInput.sqrMagnitude);
            }
            
            if (_playerMovementInput.x > 0 && _playerMovementInput.y < _playerMovementInput.x)
            {
                interactor.localPosition = new Vector3(0.5f, -0.1f, 0);
                hands.localPosition = new Vector3(-0.06f, -0.315f, 0);
            }
            else if (_playerMovementInput.x < 0 && _playerMovementInput.y > _playerMovementInput.x)
            {
                interactor.localPosition = new Vector3(-0.5f, -0.1f, 0);
                hands.localPosition = new Vector3(0.06f, -0.315f, 0);
            }
            else if (_playerMovementInput.y > 0 && _playerMovementInput.x < _playerMovementInput.y)
            {
                interactor.localPosition = new Vector3(0.0f, 0.7f, 0);
                hands.localPosition = new Vector3(-0.25f, -0.25f, 0);
            }
            else if (_playerMovementInput.y < 0 && _playerMovementInput.x > _playerMovementInput.y)
            {
                interactor.localPosition = new Vector3(0.0f, -0.7f, 0);
                hands.localPosition = new Vector3(0.25f, -0.25f, 0);
            }
        }

        private bool TryMove(Vector2 direction)
        {
            var count = _playerRigidbody.Cast(_playerMovementInput, movementFilter, _castCollisions,
                playerSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                _playerRigidbody.MovePosition(_playerRigidbody.position + direction * (playerSpeed * Time.fixedDeltaTime));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TakeDamage()
        {
            _playerState = PlayerState.NormalMode;
        }

        public void PickUpTrash()
        {
            _playerState = PlayerState.TrashMode;
        }

        public void HelpOldMan()
        {
            _playerState = PlayerState.OldManMode;
        }

        public void CallPolice()
        {
            _playerState = PlayerState.PoliceMode;
        }
        
    }
}
