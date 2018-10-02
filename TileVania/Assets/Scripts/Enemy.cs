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

    [SerializeField] GameObject scoreCanvasPrefab;
    [SerializeField] AudioClip hitSound;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    GameObject scoreCanvas;
    bool playerIsDead = false;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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
        FindObjectOfType<Player>().Score();
        scoreCanvas = Instantiate(scoreCanvasPrefab, transform.position, Quaternion.identity);
        scoreCanvas.GetComponentInChildren<Text>().text = "+" + hitScore.ToString();
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        myAnimator.SetTrigger("Die");
        FindObjectOfType<GameSession>().AddToScore(hitScore);
        Destroy(GetComponentInChildren<EnemyPlayerTrigger>().gameObject);
        Destroy(GetComponentInChildren<EnemyGroundTrigger>().gameObject);
        // EnemyDestroyTrigger must be destroyed in the next frame, otherwise the player script will not register the hit (and so player doesn't jump)
        StartCoroutine(DestroyDestroyTrigger());
        Destroy(gameObject, 1f);
    }

    public void PlayerGotDeadlyHit() {
        FindObjectOfType<GameSession>().PlayerGotDeadlyHit();
        FindObjectOfType<Player>().Die();
    }

    private IEnumerator DestroyDestroyTrigger() {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Destroy(GetComponentInChildren<EnemyDestroyTrigger>().gameObject);
    }

}
