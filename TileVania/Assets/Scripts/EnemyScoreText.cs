using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScoreText : MonoBehaviour
{
    [SerializeField] float textVelocity = 5f;
    [SerializeField] float destroyTime = 3f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, textVelocity);
        Destroy(gameObject, destroyTime);
    }

}
