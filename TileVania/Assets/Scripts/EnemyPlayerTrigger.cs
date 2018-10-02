using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerTrigger : MonoBehaviour {

    [SerializeField] Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>() && !FindObjectOfType<GameSession>().PlayerIsDead) {
            enemy.PlayerGotDeadlyHit();
        }
    }
}
