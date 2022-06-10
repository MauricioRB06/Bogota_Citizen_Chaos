
using Managers;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        
        [SerializeField] private GameObject playerPrefabP1M;
        [SerializeField] private GameObject playerPrefabP1W;
        [SerializeField] private GameObject playerPrefabP2M;
        [SerializeField] private GameObject playerPrefabP2W;
        [SerializeField] private GameObject player1SpawnPoint;
        [SerializeField] private GameObject player2SpawnPoint;
        [SerializeField] private GameObject cameraLimits;
         
        private PlayerInput _player1;
        private PlayerInput _player2;
        
        private void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
            var player1Selection = GameManager.Instance.GetPlayer1Character == 0 ? playerPrefabP1M : playerPrefabP1W;
            var player2Selection = GameManager.Instance.GetPlayer2Character == 0 ? playerPrefabP2M : playerPrefabP2W;
            
            _player1 = PlayerInput.Instantiate(prefab: player1Selection, playerIndex: 0,
                controlScheme : "Player_1", pairWithDevice: Keyboard.current, splitScreenIndex: 0);
            
            _player2 = PlayerInput.Instantiate(prefab: player2Selection, playerIndex: 1,
                controlScheme : "Player_2", pairWithDevice: Keyboard.current, splitScreenIndex: 1);

            var positionPlayer1 = player1SpawnPoint.transform.position;
            var xSpawnPositionPlayer1 = (int) positionPlayer1.x;
            var ySpawnPositionPlayer1 = (int) positionPlayer1.y;

            var positionPlayer2 = player2SpawnPoint.transform.position;
            var xSpawnPositionPlayer2 = (int) positionPlayer2.x;
            var ySpawnPositionPlayer2 = (int) positionPlayer2.y;
            
            _player1.transform.position = new Vector3(-5, 0, 0);
            _player1.GetComponent<PlayerController>().SetPlayerIndex(_player1.playerIndex,xSpawnPositionPlayer1,ySpawnPositionPlayer1, cameraLimits.GetComponent<Collider2D>());
            
            _player2.transform.position = new Vector3(5, 0, 0);
            _player2.GetComponent<PlayerController>().SetPlayerIndex(_player2.playerIndex,xSpawnPositionPlayer2,ySpawnPositionPlayer2, cameraLimits.GetComponent<Collider2D>());
        }

        public PlayerInput GetPlayer1Reference => _player1;
        public PlayerInput GetPlayer2Reference => _player2;
        
    }
}
