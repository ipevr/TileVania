using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField] int pointsPerCoin = 10;

    [Header("Audio Files")]
    [SerializeField] AudioClip clip;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>()) {
            FindObjectOfType<GameSession>().AddToCoins(pointsPerCoin);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }

}
