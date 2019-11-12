using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
    private bool startedCollision = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(!startedCollision)
        {
            AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().CollectCoin();
            Destroy(gameObject);
        }
        startedCollision = true;
    }
}
