using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    [SerializeField] float runSpeed = 3f;
    [SerializeField] float climbSpeedHorizontal = 1f;
    [SerializeField] float climbSpeedVertical = 1f;
    [SerializeField] float jumpVerticalPower = 7f;
    [SerializeField] float jumpHorizontalPower = 1.3f;

    bool jumping = false;

    Rigidbody2D myRigidbody;
    Collider2D myCollider;
    Animator myAnimator;
    float gravityScaleAtStart = 0f;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
        gravityScaleAtStart = myRigidbody.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (!jumping) {
            Run();
        }
        Jump();
        ClimbLadder();
    }

    private void Jump() {
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) {
            Debug.Log("Jump on");
            jumping = true;
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !jumping) {
            Debug.Log("Jump");
            gameObject.transform.position += new Vector3(0f, 0.01f, 0f); // give player a small vertical shift to prevent switching jumping to false in the next frame because player is still touching ground layer
            Vector2 jumpVelocity = new Vector2(jumpHorizontalPower * Mathf.Sign(myRigidbody.velocity.x), jumpVerticalPower);
            myRigidbody.velocity = jumpVelocity;
            jumping = true;
            return;
        }
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) {
            Debug.Log("Jump off");
            jumping = false;
        }
    }

    private void ClimbLadder() {
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
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
