
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
        
        private string _player1Name = "Player 1";
        private string _player2Name = "Player 2";

        private bool _pause;
        private GameObject _pauseScreen;

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

            _pause = false;
        }

        public void GamePause()
        {
            _pause = !_pause;
            Time.timeScale = _pause ? 0 : 1;
            if(_pauseScreen == null)
                _pauseScreen = GameObject.FindGameObjectWithTag("Pause");
            _pauseScreen.SetActive(_pause);
        }
        
        public void LoadMap(int map)
        {
            SceneManager.LoadScene(map == 0 ? "City" : "Testing");
        }
        
        public void SetPlayer1Character(int character)
        {
            _player1Character = character;
        }
        
        public void SetPlayer2Character(int character)
        {
            _player2Character = character;
        }

        public void SerPlayer1Name(string playerName)
        {
            _player1Name = playerName;
        }
        
        public void SerPlayer2Name(string playerName)
        {
            _player2Name = playerName;
        }
        
        public int GetPlayer1Character => _player1Character;
        public int GetPlayer2Character => _player2Character;

        public int GetPlayer1Skin()
        {
            return _player1Character;
        }
        
        public int GetPlayer2Skin()
        {
            return _player2Character;
        }
        
        public string GetPlayer1Name()
        {
            return _player1Name;
        }
        public string GetPlayer2Name()
        {
           return _player2Name;
        }
        
    }
}
