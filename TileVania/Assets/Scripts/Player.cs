using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    enum Direction { left, right };

    [SerializeField]
    private float runSpeed = 1f;

    Rigidbody2D myRigidbody;
    Animator animator;
    Direction direction = Direction.right;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0) { 
            Run();
        } else {
            DoNothing();
        }
    }

    private void DoNothing() {
        myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        animator.SetTrigger("Idle");
    }

    private void Run() {
        // control velocity
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        // control direction

        if (controlThrow < 0f) {
            Turn(Direction.left);
        } else {
            Turn(Direction.right);
        }
        // control animation
        animator.SetTrigger("Running");
    }

    private void Turn(Direction direction) {
        float xScale = 1f;
        if (direction == Direction.right) {
            xScale = Mathf.Abs(gameObject.transform.localScale.x);
        } else {
            xScale = -Mathf.Abs(gameObject.transform.localScale.x);
        }
        float yScale = gameObject.transform.localScale.y;
        float zScale = gameObject.transform.localScale.z;
        gameObject.transform.localScale = new Vector3(xScale, yScale, zScale);
    }

    private void Flip() {
        float xScale = -gameObject.transform.localScale.x;
        float yScale = gameObject.transform.localScale.y;
        float zScale = gameObject.transform.localScale.z;
        gameObject.transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}
