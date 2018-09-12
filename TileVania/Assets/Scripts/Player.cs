using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    [SerializeField] float runSpeed = 3f;
    [SerializeField] float climbSpeedHorizontal = 1f;
    [SerializeField] float climbSpeedVertical = 1f;
    [SerializeField] float jumpVerticalPower = 7f;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    float gravityScaleAtStart = 0f;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        gravityScaleAtStart = myRigidbody.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        Run();
        Jump();
        ClimbLadder();
    }

    private void Jump() {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) {
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump")) {
            Vector2 jumpAddVelocity = new Vector2(0f, jumpVerticalPower);
            myRigidbody.velocity += jumpAddVelocity;
            return;
        }
    }

    private void ClimbLadder() {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myAnimator.SetBool("Climbing", false);
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }
        myRigidbody.gravityScale = 0f;
        float controlThrowY = CrossPlatformInputManager.GetAxis("Vertical");
        float controlThrowX = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrowX * climbSpeedHorizontal, controlThrowY * climbSpeedVertical);
        myRigidbody.velocity = playerVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Run() {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
        Flip();
    }

    private void Flip() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) {
            gameObject.transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

}
