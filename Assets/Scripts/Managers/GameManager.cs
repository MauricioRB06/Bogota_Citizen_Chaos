using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        private int _player1Character;
        private int _player2Character;
        
        private int _player1Score;
        private int _player2Score;
        
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
        }

        public void LoadMap(int map)
        {
            SceneManager.LoadScene(map == 0 ? "`City" : "Testing");
        }
        
        public void SetPlayer1Character(int character)
        {
            _player1Character = character;
            Debug.Log("Player 1 character set to " + _player1Character);
        }
        
        public void SetPlayer2Character(int character)
        {
            _player2Character = character;
            Debug.Log("Player 2 character set to " + _player2Character);
        }
        
        public int GetPlayer1Character => _player1Character;
        public int GetPlayer2Character => _player2Character;
        
    }
}
