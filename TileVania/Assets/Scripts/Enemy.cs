using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public enum Facing { right, left };

    [SerializeField] float speed = 2f;
    [SerializeField] int hitScore = 200;
    [SerializeField] Facing isFacing;
    [SerializeField] GameObject scoreCanvas;

    Rigidbody2D myRigidbody;
    Animator myAnimator;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        scoreCanvas.GetComponentInChildren<Text>().text = "+" + hitScore.ToString();
        scoreCanvas.SetActive(false);
        if (speed >= 0 && isFacing == Facing.left) {
            transform.localScale = new Vector2(-transform.localScale.x, 1f);
        }
        myRigidbody.velocity = new Vector2(speed, 0f);
    }

    public void ChangeDirection() {
        speed = -speed;
        myRigidbody.velocity = new Vector2(speed, 0f);
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

    public void GotDeadlyHit() {
        myRigidbody.velocity = Vector2.zero;
        scoreCanvas.SetActive(true);
        myAnimator.SetTrigger("Die");
        FindObjectOfType<GameSession>().AddToScore(hitScore);
        Destroy(GetComponentInChildren<EnemyPlayerTrigger>().gameObject);
        StartCoroutine(DestroyDestroyTrigger());
        StartCoroutine(Die());
    }

    private IEnumerator DestroyDestroyTrigger() {
        yield return new WaitForEndOfFrame();
        Destroy(GetComponentInChildren<EnemyDestroyTrigger>().gameObject);
    }

    private IEnumerator Die() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }

}
