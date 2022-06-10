using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private TextMeshProUGUI scoreP1;
        [SerializeField] private TextMeshProUGUI scoreP2;

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