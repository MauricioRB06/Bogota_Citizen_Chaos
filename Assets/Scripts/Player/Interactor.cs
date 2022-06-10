
using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactor : MonoBehaviour
    {
        public BoxCollider2D interactorCollider;
        private bool _canInteract;
        private int _attackX;
        private int _attackY;
        
        private void Awake()
        {
            interactorCollider = GetComponent<BoxCollider2D>();
            interactorCollider.isTrigger = true;
            interactorCollider.enabled = false;
            _canInteract = true;
        }

        public void EnableInteractor(int x, int y)
        {
            if (!_canInteract) return;
            
            _attackX = x;
            _attackY = y;
            
            interactorCollider.enabled = true;
            _canInteract = false;
            StartCoroutine(DisableInteractor());
        }

        private IEnumerator DisableInteractor()
        {
            yield return new WaitForSeconds(0.25f);
            interactorCollider.enabled = false;
            _canInteract = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var colliderObject = collision.GetComponent<IPlayer>();
            colliderObject?.TakeDamage(_attackX, _attackY);
        }
        
    }
}
