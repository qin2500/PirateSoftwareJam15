using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings settings;
    private Rigidbody2D rb;
    private CapsuleCollider2D collider;
    private Vector2 curVelocity;
    private bool cachedQueryStartInColliders;
    private InputData inputData;
    private float timeAC;

    //Jumping
    private bool isGrounded;
    private bool coyoteOn;
    private bool jumpEndedEarly;
    private bool canJumpBuffer;
    private bool jumping;
    private float jumpTime;
    private float ungroundedTime = float.MinValue;

    //Swimming

    public struct InputData
    {
        public float horizonatal;
        public float vertical;
        public bool jumpPressed;
        public bool jumpHeld;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider= GetComponent<CapsuleCollider2D>();

        cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Update()
    {
        timeAC += Time.deltaTime;
        inputData = new InputData
        {
            horizonatal = Input.GetAxisRaw("Horizontal"),
            vertical = Input.GetAxisRaw("Vertical"),
            jumpPressed = Input.GetButtonDown("Jump"),
            jumpHeld = Input.GetButton("Jump"),
        };
        if (inputData.jumpPressed)
        {
            jumping = true;
            jumpTime = timeAC;
        }
        
    }

    private void FixedUpdate()
    {
        CheckCollision();

        jump();
        movementHandler();
        gravity();

        rb.velocity = curVelocity;
    }

    private void CheckCollision()
    {

        Physics2D.queriesStartInColliders = false;
        bool groundHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.down, settings.groundedDistance, settings.GoundLayer);
        bool ceilingHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.up, settings.groundedDistance, settings.GoundLayer); ;

        if (ceilingHit) curVelocity.y = Math.Min(0, curVelocity.y);

        if(!isGrounded && groundHit)
        {
            isGrounded = true;
            coyoteOn = true;
            canJumpBuffer = true;
            jumpEndedEarly = false;
        }
        else if (isGrounded && !groundHit)
        {
            isGrounded = false;
            ungroundedTime = timeAC;
        }
        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;

    }

    private void movementHandler()
    {
        if(inputData.horizonatal == 0)
        {
            var acc = isGrounded ? settings.groundDeceleration : settings.airDeceleration;
            curVelocity.x = Mathf.MoveTowards(curVelocity.x, 0, acc * Time.fixedDeltaTime);
        }
        else
        {
            curVelocity.x = Mathf.MoveTowards(curVelocity.x, inputData.horizonatal * settings.maxSpeed, settings.acceleration * Time.fixedDeltaTime);
        }
    }
    private void gravity()
    {
        if (isGrounded && curVelocity.y <= 0f)
        {
            curVelocity.y = settings.groundingForce;
        }
        else
        {
            var airGrav = settings.fallAcceleration;
            if (jumpEndedEarly && curVelocity.y > 0)airGrav  *= settings.jumpEndEarlyGravityModifier;

            curVelocity.y = Mathf.MoveTowards(curVelocity.y, -settings.maxFallSpeed, airGrav * Time.fixedDeltaTime);
        }
    }

    private void jump()
    {
        if (!jumpEndedEarly && !isGrounded && !inputData.jumpHeld && rb.velocity.y > 0) jumpEndedEarly = true;

        if (!jumping && !(canJumpBuffer && timeAC < jumpTime + settings.jumpBuffer)) return;

        if (isGrounded || (coyoteOn && !isGrounded && timeAC < ungroundedTime + settings.coyoteTime)) ExecuteJump();

        jumping = false;
    }
    private void ExecuteJump()
    {
        Debug.Log("Penis");
        jumpEndedEarly = false;
        jumpTime = 0;
        canJumpBuffer = false;
        coyoteOn = false;
        curVelocity.y = settings.jumpPower;
    }
}
