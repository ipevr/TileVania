using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpVerticalPower = 5f;
    [SerializeField] float jumpHorizontalPower = 2f;

    bool jumping = false;

    Rigidbody2D myRigidbody;
    Collider2D myCollider;
    Animator myAnimator;
    string[] jumpLayerNames = new string[1];
    int jumpLayerMask;

    // Use this for initialization
    void Start () {
        jumpLayerNames[0] = "Ground";
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
        jumpLayerMask = LayerMask.GetMask(jumpLayerNames);
	}
	
	// Update is called once per frame
	void Update () {
        if (!jumping) {
            Run();
        }
        Jump();
    }

    private void Jump() {
        if (!myCollider.IsTouchingLayers(jumpLayerMask)) {
            jumping = true;
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !jumping) {
            Vector2 jumpVelocity = new Vector2(jumpHorizontalPower * Mathf.Sign(myRigidbody.velocity.x), jumpVerticalPower);
            myRigidbody.velocity = jumpVelocity;
            return;
        }
        if (myCollider.IsTouchingLayers(jumpLayerMask)) {
            jumping = false;
        }
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
