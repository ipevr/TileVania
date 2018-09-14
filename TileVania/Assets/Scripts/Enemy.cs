using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    const int layerPlayer = 10;

    public enum Facing { right, left };

    [SerializeField] float speed = 2f;
    [SerializeField] Facing isFacing;

    Rigidbody2D myRigidbody;
    BoxCollider2D myGroundCollider;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myGroundCollider = GetComponent<BoxCollider2D>();
        if (speed >= 0 && isFacing == Facing.left) {
            transform.localScale = new Vector2(-transform.localScale.x, 1f);
        }
        myRigidbody.velocity = new Vector2(speed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer != layerPlayer) {
            speed = -speed;
            myRigidbody.velocity = new Vector2(speed, 0f);
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
        }
    }

}
