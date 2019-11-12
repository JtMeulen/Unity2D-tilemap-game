using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float climbSpeed = 6f;

    // Cached
    Rigidbody2D myRigidbody;
    BoxCollider2D myColliderBody;
    PolygonCollider2D myColliderFeet;
    Animator myAnimator;
    float myGravity;

    // State
    bool isAlive = true;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myColliderBody = GetComponent<BoxCollider2D>();
        myColliderFeet = GetComponent<PolygonCollider2D>();
        myAnimator = GetComponent<Animator>();
        myGravity = myRigidbody.gravityScale;
    }

    private void Update()
    {
        if (isAlive)
        {
            Run();
            Climb();
            Jump();
            Die();
            FlipSprite();
        }
    }

    private void Run()
    {
        float xInput = Input.GetAxis("Horizontal");
        myRigidbody.velocity = new Vector2(xInput * runSpeed, myRigidbody.velocity.y);

        myAnimator.SetBool("IsRunning", isMovingHorizontally());
    }

    private void Climb()
    {
        if (!myColliderBody.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = myGravity;
            myAnimator.SetBool("IsClimbing", false);
            return;
        }

        myRigidbody.gravityScale = 0;
        myAnimator.SetBool("IsRunning", false);
        myAnimator.SetBool("IsClimbing", true);
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxis("Vertical") * climbSpeed);

    }

    private void Jump()
    {
        if (!Input.GetButtonDown("Jump")) { return; }

        // Only jump when touching "Ground" layer
        if (myColliderFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
        }
    }

    private void Die()
    {
        if (myColliderBody.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetBool("isDead", true);
            myRigidbody.velocity = new Vector2(0, 15f);
            isAlive = false;

            StartCoroutine(respawn());
        }
    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameSession>().LoseLife();
    }

    private void FlipSprite()
    {
        if (isMovingHorizontally())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private bool isMovingHorizontally()
    {
        return Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
    }
}
