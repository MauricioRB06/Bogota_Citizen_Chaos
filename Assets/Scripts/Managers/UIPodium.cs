using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UIPodium : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerWinner1Case1;
        [SerializeField] private TextMeshProUGUI playerWinner1Case2;
        [SerializeField] private TextMeshProUGUI playerWinner1Case3;
        [SerializeField] private TextMeshProUGUI playerWinner2Case3;
        
        [SerializeField] private Sprite spriteWinnerP1M;
        [SerializeField] private Sprite spriteWinnerP2M;
        [SerializeField] private Sprite spriteWinnerP1W;
        [SerializeField] private Sprite spriteWinnerP2W;
        
        [SerializeField] private GameObject spriteWinnerCase1;
        [SerializeField] private GameObject spriteWinnerCase2;
        [SerializeField] private GameObject spriteWinner1Case3;
        [SerializeField] private GameObject spriteWinner2Case3;
        
        [SerializeField] private GameObject finalCase1;
        [SerializeField] private GameObject finalCase2;
        [SerializeField] private GameObject finalCase3;
        
        private string _player1;
        private string _player2;
        
        public void Podium(PlayerInput player1Reference, PlayerInput player2Reference)
        {
            _player1 = GameManager.Instance.GetPlayer1Name();
            _player2 = GameManager.Instance.GetPlayer2Name();

            var score1 = player1Reference.GetComponent<PlayerController>().GetScore();
            var score2 = player2Reference.GetComponent<PlayerController>().GetScore();

            if (score1 > score2)
            {
                playerWinner1Case1.text = _player1;

                if (GameManager.Instance.GetPlayer1Skin() == 0)
                {
                    spriteWinnerCase1.GetComponent<Image>().sprite = spriteWinnerP1M;
                }
                else
                {
                    spriteWinnerCase1.GetComponent<Image>().sprite = spriteWinnerP1W;
                }

                finalCase1.SetActive(true);
            }
            else if (score1 < score2)
            {
                playerWinner1Case2.text = _player2;
                
                if (GameManager.Instance.GetPlayer2Skin() == 0)
                {
                    spriteWinnerCase2.GetComponent<Image>().sprite = spriteWinnerP2M;
                }
                else
                {
                    spriteWinnerCase2.GetComponent<Image>().sprite = spriteWinnerP2W;
                }
                
                finalCase2.SetActive(true);
            }
            else if (score1 == score2)
            {
                playerWinner1Case3.text = _player1;
                playerWinner2Case3.text = _player2;
                
                if (GameManager.Instance.GetPlayer1Skin() == 0)
                {
                    spriteWinner1Case3.GetComponent<Image>().sprite = spriteWinnerP1M;
                }
                else
                {
                    spriteWinner1Case3.GetComponent<Image>().sprite = spriteWinnerP1W;
                }
                
                if (GameManager.Instance.GetPlayer2Skin() == 0)
                {
                    spriteWinner2Case3.GetComponent<Image>().sprite = spriteWinnerP2M;
                }
                else
                {
                    spriteWinner2Case3.GetComponent<Image>().sprite = spriteWinnerP2W;
                }
                
                finalCase3.SetActive(true);
            }
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}
