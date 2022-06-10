using Player;
using UnityEngine;

public class TestingDead : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        var colliderObject = collision.GetComponent<IPlayer>();
        colliderObject?.Dead(0,-3);
    }
    
}
