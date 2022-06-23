
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Managers;
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
        [SerializeField] private GameObject interactor;
        [SerializeField] private Transform hands;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
        
        private PlayerState _playerState;
        private Vector2 _playerMovementInput;
        private Animator _playerAnimator;
        private Rigidbody2D _playerRigidbody;
        private List<RaycastHit2D> _castCollisions = new();
        private Vector2 _playerSpawnPosition;
        private int _playerScore;

        private bool _canMove = true;
        private int _playerIndex;
        
        private static readonly int Interact = Animator.StringToHash("Interact");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int HitTrigger = Animator.StringToHash("Hit");
        private static readonly int DeadTrigger = Animator.StringToHash("Dead");

        // 
        private void Awake()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
            _playerState = PlayerState.NormalMode;
        }
        
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.GamePause();
            }
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
                
                if(interactor.transform.localPosition == new Vector3(0.5f, -0.1f, 0))
                {
                    interactor.GetComponent<Interactor>().EnableInteractor(10,0);
                }
                else if(interactor.transform.localPosition == new Vector3(-0.5f, -0.1f, 0))
                {
                    interactor.GetComponent<Interactor>().EnableInteractor(-10,0);
                }
                else if(interactor.transform.localPosition == new Vector3(0.0f, 0.2f, 0))
                {
                    interactor.GetComponent<Interactor>().EnableInteractor(0,10);
                }
                else if(interactor.transform.localPosition == new Vector3(0.0f, -0.675f, 0))
                {
                    interactor.GetComponent<Interactor>().EnableInteractor(0,-10);
                }
                
            }
        }
        
        // Rigidbody Movement
        public void FixedUpdate()
        {
            if(_canMove){
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

                switch (_playerMovementInput.x)
                {
                    case > 0 when _playerMovementInput.y < _playerMovementInput.x:
                        interactor.transform.localPosition = new Vector3(0.5f, -0.1f, 0);
                        hands.localPosition = new Vector3(-0.06f, -0.315f, 0);
                        interactor.GetComponent<Interactor>().interactorCollider.size = new Vector2(0.3f, 0.75f);
                        break;
                    case < 0 when _playerMovementInput.y > _playerMovementInput.x:
                        interactor.transform.localPosition = new Vector3(-0.5f, -0.1f, 0);
                        hands.localPosition = new Vector3(0.06f, -0.315f, 0);
                        interactor.GetComponent<Interactor>().interactorCollider.size = new Vector2(0.3f, 0.75f);
                        break;
                    default:
                    {
                        switch (_playerMovementInput.y)
                        {
                            case > 0 when _playerMovementInput.x < _playerMovementInput.y:
                                interactor.transform.localPosition = new Vector3(0.0f, 0.2f, 0);
                                hands.localPosition = new Vector3(-0.25f, -0.25f, 0);
                                interactor.GetComponent<Interactor>().interactorCollider.size = new Vector2(0.75f, 0.6f);
                                break;
                            case < 0 when _playerMovementInput.x > _playerMovementInput.y:
                                interactor.transform.localPosition = new Vector3(0.0f, -0.675f, 0);
                                hands.localPosition = new Vector3(0.25f, -0.25f, 0);
                                interactor.GetComponent<Interactor>().interactorCollider.size = new Vector2(0.75f, 0.3f);
                                break;
                        }

                        break;
                    }
                }
                
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

        public void SetPlayerIndex(int index, int xSpawnPosition, int ySpawnPosition, Collider2D cameraLimit)
        {
            _playerIndex = index;
            playerCamera.rect = _playerIndex == 0 ? new Rect(0, 0, 0.5f, 1) : new Rect(0.5f, 0, 0.5f, 1);
            
            _playerSpawnPosition.x = xSpawnPosition;
            _playerSpawnPosition.y = ySpawnPosition;
            GetComponentInParent<Transform>().position = _playerSpawnPosition;

            playerVirtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = cameraLimit;
        }
        
        public void LockMovement() { _canMove = false; }
        private void UnlockMovement() { _canMove = true; }
        
        private IEnumerator StopDamage()
        {
            yield return new WaitForSeconds(0.1f);
            _playerRigidbody.velocity = new Vector2(0,0);
        }

        public int GetScore()
        {
            return _playerScore;
        }
        
        public void TakeDamage(int positionX, int positionY)
        {
            _playerState = PlayerState.NormalMode;
            _playerAnimator.SetTrigger(HitTrigger);
            _playerRigidbody.velocity = new Vector2(positionX,positionY);
            StartCoroutine(StopDamage());
        }
        
        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(0.7f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _playerRigidbody.velocity = new Vector2(0,0);
            _playerRigidbody.MovePosition(_playerSpawnPosition);
            yield return new WaitForSeconds(5);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            _canMove = true;
        }
        
        public void Dead(int xPosition, int yPosition)
        {
            _canMove = false;
            _playerAnimator.SetTrigger(DeadTrigger);
            _playerRigidbody.velocity = new Vector2(xPosition,yPosition);
            StartCoroutine(Spawn());
        }

        public void UpdateScore(bool actionType, int score)
        {
            if(actionType)
            {
                _playerScore += score;
            }
            else
            {
                _playerScore -= score;
            }
            
            UIManager.Instance.SetScore(_playerIndex, _playerScore);
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
