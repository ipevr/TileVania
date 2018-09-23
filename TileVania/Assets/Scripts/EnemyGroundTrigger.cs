using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundTrigger : MonoBehaviour {

    [SerializeField] Enemy enemy;

    private void OnTriggerExit2D(Collider2D collision) {
        enemy.ChangeDirection();
    }
}
