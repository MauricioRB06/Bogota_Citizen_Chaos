using TMPro;
using UnityEngine;

namespace Managers
{
    public class PlayerNameSpawnPoint : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;
        
        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            Debug.Log(_textMeshPro.text);
        }
        
    }
}
