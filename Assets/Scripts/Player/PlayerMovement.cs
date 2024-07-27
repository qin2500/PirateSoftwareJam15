using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings settings;
    private Rigidbody2D rb;
    private new CapsuleCollider2D collider;
    private Vector2 curVelocity;
    private bool cachedQueryStartInColliders;
    private InputData inputData;
    private float timeAC;

    //Jumping
    private bool isGrounded;
    private bool coyoteOn;
    private bool jumpEndedEarly;
    private bool canJumpBuffer;
    [SerializeField]private bool jumping;
    private float jumpTime;
    private float ungroundedTime = float.MinValue;

    //Swimming
    private bool onShadow = false;
    [SerializeField]private bool swimming;
    [SerializeField]private bool shadowJump;

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

        shadow();
        jump();
        movementHandler();
        gravity();

        rb.velocity = curVelocity;
    }

    private void CheckCollision()
    {

        Physics2D.queriesStartInColliders = false;
        
        bool ceilingHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.up, settings.groundedDistance, settings.GoundLayer); ;
        if (ceilingHit) curVelocity.y = Math.Min(0, curVelocity.y);

        RaycastHit2D groundHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.down, settings.groundedDistance, settings.GoundLayer);

        if (!isGrounded && groundHit)
        {
            isGrounded = true;
            coyoteOn = true;
            canJumpBuffer = true;
            jumpEndedEarly = false;
            shadowJump = false;
        }
        else if (isGrounded && !groundHit)
        {
            isGrounded = false;
            ungroundedTime = timeAC;
        }
        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.transform.CompareTag("Shadow"))
        {
            onShadow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.transform.CompareTag("Shadow"))
        {
            onShadow = false;
        }
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
            if (swimming)
               curVelocity.x = Mathf.MoveTowards(curVelocity.x, inputData.horizonatal * settings.swimSpeed, settings.acceleration * Time.fixedDeltaTime);
            else if(shadowJump)
                curVelocity.x = Mathf.MoveTowards(curVelocity.x, inputData.horizonatal * settings.maxSpeed * 0.1f, settings.shadowJumpHorizontalAcceleration * Time.fixedDeltaTime);
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
        if (!jumpEndedEarly && !isGrounded && !inputData.jumpHeld && rb.velocity.y > 0 && !shadowJump) jumpEndedEarly = true;

        if (!jumping && !(canJumpBuffer && timeAC < jumpTime + settings.jumpBuffer)) return;

        if (isGrounded || (coyoteOn && !isGrounded && timeAC < ungroundedTime + settings.coyoteTime)) ExecuteJump();

        jumping = false;
    }
    private void ExecuteJump()
    {
        jumpEndedEarly = false;
        jumpTime = 0;
        canJumpBuffer = false;
        coyoteOn = false;

        if (swimming)
        {
            swimming = false;
            shadowJump = true;
        }
        

        if (!shadowJump)
            curVelocity.y = settings.jumpPower;
        else
        {
            curVelocity.y = settings.shadowJumpPower;
        }
        
    }

    private void shadow()
    {
        if(inputData.vertical < 0 && onShadow)
        {
            swimming = true;
            onShadow = false;
        }
    }

    public bool getSwimming()
    {
        return swimming;
    }

    public Vector2 getCurVelocity()
    {
        return curVelocity;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }
}
