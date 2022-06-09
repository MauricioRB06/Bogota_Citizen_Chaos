
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // 
    [RequireComponent(typeof(Rigidbody2D))]
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float playerSpeed = 5f;
        
        private Vector2 _playerMovement;
        private Rigidbody2D _playerRigidbody;
        
        // 
        private void Awake()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
        }

        // 
        public void OnMove(InputAction.CallbackContext context)
        {
            _playerMovement = context.ReadValue<Vector2>();
        }
        
        // Rigidbody Movement
        public void FixedUpdate()
        {
            _playerRigidbody.velocity = _playerMovement * playerSpeed * Time.fixedDeltaTime;
        }
        
    }
}
