using System.Collections;
using Input;
using Player;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private TextMeshProUGUI scoreP1;
        [SerializeField] private TextMeshProUGUI scoreP2;
        [SerializeField] private GameObject endGameUI;
        [SerializeField] private GameObject normalGameplayUI;
        [SerializeField] private GameObject podiumScoreUI;
        
        [SerializeField] private float timerDuration = 3f * 60f; //Duration of the timer in seconds
        [SerializeField] private bool countDown = true;

        private float timer;
        private float flashTimer;
        private bool finishGame;
        
        [SerializeField] private TextMeshProUGUI firstMinute;
        [SerializeField] private TextMeshProUGUI secondMinute;
        [SerializeField] private TextMeshProUGUI separator;
        [SerializeField] private TextMeshProUGUI firstSecond;
        [SerializeField] private TextMeshProUGUI secondSecond;
        [SerializeField] private float flashDuration = 1f; //The full length of the flash
        
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
        
        private void Start() {
            ResetTimer();
        }
        
        public void SetScore(int playerIndex, int score)
        {
            if(playerIndex == 0)
            {
                scoreP1.text = score.ToString();
            }
            else
            {
                scoreP2.text = score.ToString();
            }
        }
        
        private void ResetTimer() {
            if (countDown) {
                timer = timerDuration;
            } else {
                timer = 0;
            }
            SetTextDisplay(true);
        }
        
        void Update() {
            if (countDown && timer > 0) {
                timer -= Time.deltaTime;
                UpdateTimerDisplay(timer);
            } else if (!countDown && timer < timerDuration) {
                timer += Time.deltaTime;
                UpdateTimerDisplay(timer);
            } else {
                FlashTimer();
                if (finishGame) return;
                finishGame = true;
                StartCoroutine(EndGame());
            }
        }

        private void UpdateTimerDisplay(float time) {
            if (time < 0) {
                time = 0;
            }

            if (time > 3660) {
                Debug.LogError("Timer cannot display values above 3660 seconds");
                ErrorDisplay();
                return;
            }

            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);

            string currentTime = string.Format("{00:00}{01:00}", minutes, seconds);
            firstMinute.text = currentTime[0].ToString();
            secondMinute.text = currentTime[1].ToString();
            firstSecond.text = currentTime[2].ToString();
            secondSecond.text = currentTime[3].ToString();
        }

        private void ErrorDisplay() {
            firstMinute.text = "8";
            secondMinute.text = "8";
            firstSecond.text = "8";
            secondSecond.text = "8";
        }

        private void FlashTimer() {
            if(countDown && timer != 0) {
                timer = 0;
                UpdateTimerDisplay(timer);
            }

            if(!countDown && timer != timerDuration) {
                timer = timerDuration;
                UpdateTimerDisplay(timer);
            }

            if(flashTimer <= 0) {
                flashTimer = flashDuration;
            } else if (flashTimer <= flashDuration / 2) {
                flashTimer -= Time.deltaTime;
                SetTextDisplay(true);
            } else {
                flashTimer -= Time.deltaTime;
                SetTextDisplay(false);
            }
        }

        private void SetTextDisplay(bool enabled) {
            firstMinute.enabled = enabled;
            secondMinute.enabled = enabled;
            separator.enabled = enabled;
            firstSecond.enabled = enabled;
            secondSecond.enabled = enabled;
        }

        private IEnumerator EndGame()
        {
            var player1 = InputManager.Instance.GetPlayer1Reference;
            var player2 = InputManager.Instance.GetPlayer2Reference;
            player1.GetComponent<PlayerController>().LockMovement();
            player2.GetComponent<PlayerController>().LockMovement();
            endGameUI.SetActive(true);
            yield return new WaitForSeconds(2f);
            normalGameplayUI.SetActive(false);
            podiumScoreUI.SetActive(true);
            podiumScoreUI.GetComponent<UIPodium>().Podium(player1, player2);
            Time.timeScale = 0;
        }
        
    }
}
