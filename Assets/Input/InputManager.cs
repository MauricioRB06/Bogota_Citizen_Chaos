
using Managers;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefabP1M;
        [SerializeField] private GameObject playerPrefabP1W;
        [SerializeField] private GameObject playerPrefabP2M;
        [SerializeField] private GameObject playerPrefabP2W;
        [SerializeField] private GameObject player1SpawnPoint;
        [SerializeField] private GameObject player2SpawnPoint;
        [SerializeField] private GameObject cameraLimits;
        
        private void Awake()
        {
            var player1Selection = GameManager.Instance.GetPlayer1Character == 0 ? playerPrefabP1M : playerPrefabP1W;
            var player2Selection = GameManager.Instance.GetPlayer2Character == 0 ? playerPrefabP2M : playerPrefabP2W;
            
            var player1 = PlayerInput.Instantiate(prefab: player1Selection, playerIndex: 0,
                controlScheme : "Player_1", pairWithDevice: Keyboard.current, splitScreenIndex: 0);
            
            var player2 = PlayerInput.Instantiate(prefab: player2Selection, playerIndex: 1,
                controlScheme : "Player_2", pairWithDevice: Keyboard.current, splitScreenIndex: 1);

            var positionPlayer1 = player1SpawnPoint.transform.position;
            var xSpawnPositionPlayer1 = (int) positionPlayer1.x;
            var ySpawnPositionPlayer1 = (int) positionPlayer1.y;

            var positionPlayer2 = player2SpawnPoint.transform.position;
            var xSpawnPositionPlayer2 = (int) positionPlayer2.x;
            var ySpawnPositionPlayer2 = (int) positionPlayer2.y;
            
            player1.transform.position = new Vector3(-5, 0, 0);
            player1.GetComponent<PlayerController>().SetPlayerIndex(player1.playerIndex,xSpawnPositionPlayer1,ySpawnPositionPlayer1, cameraLimits.GetComponent<Collider2D>());
            
            player2.transform.position = new Vector3(5, 0, 0);
            player2.GetComponent<PlayerController>().SetPlayerIndex(player2.playerIndex,xSpawnPositionPlayer2,ySpawnPositionPlayer2, cameraLimits.GetComponent<Collider2D>());
        }
        
    }
}
