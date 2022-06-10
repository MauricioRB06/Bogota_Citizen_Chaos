using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private TextMeshProUGUI scoreP1;
        [SerializeField] private TextMeshProUGUI scoreP2;
        [SerializeField] private GameObject player1Name;
        [SerializeField] private GameObject player2Name;
        
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
            
            scoreP1.text = "999";
            //player1Name.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetPlayer1Name();
           // player2Name.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetPlayer2Name();
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
        
    }
}