using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyTrigger : MonoBehaviour
{

    [SerializeField] Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>() && other.GetType() == typeof(BoxCollider2D) && !FindObjectOfType<GameSession>().PlayerIsDead)  {
            enemy.GotDeadlyHit();
        }
    }

}
