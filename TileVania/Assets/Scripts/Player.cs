using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    const int layerEnemy = 12;

    [SerializeField] float runSpeed = 3f;
    [SerializeField] float climbSpeedHorizontal = 1f;
    [SerializeField] float climbSpeedVertical = 1f;
    [SerializeField] float jumpVerticalPower = 7f;
    [SerializeField] float deadVerticalPower = 10f;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    float gravityScaleAtStart = 0f;
    bool isImmobile = false;
    int finalSceneIndex;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        finalSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        if (SceneManager.GetActiveScene().buildIndex == finalSceneIndex) {
            myAnimator.SetTrigger("BeingHappy");
            isImmobile = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isImmobile) { return; }
        Run();
        Flip();
        Jump();
        ClimbLadder();
        Die();
        BeHappy();
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
    }

    private void Flip() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) {
            gameObject.transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void Die() {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Traps"))) {
            myAnimator.SetTrigger("Dying");
            myRigidbody.gravityScale = gravityScaleAtStart;
            Vector2 deadVelocity = new Vector2(0f, deadVerticalPower);
            myRigidbody.velocity = deadVelocity;
            isImmobile = true;
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private void BeHappy() {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("LevelExit"))) {
            myAnimator.SetTrigger("BeingHappy");
            Vector2 happyVelocity = new Vector2(0.5f, 2f);
            myRigidbody.velocity = happyVelocity;
            isImmobile = true;
        }
    }

    private IEnumerator HandlePlayerDeath() {
        yield return new WaitForSeconds(2);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

}
