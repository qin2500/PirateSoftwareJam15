
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class GroundedFollowPlayer : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int exp;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rayLength = 0.5f;

    private Rigidbody2D rb;
    private Vector2 curVelocity;
    private int playerDir = 0;
    private EnemyHealth enemyHealth;
    private bool cachedQueryStartInColliders;
    private bool isGrounded;
    private CapsuleCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth= GetComponent<EnemyHealth>();
        enemyHealth.exp = exp;
        cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GlobalReferences.PLAYER.PlayerObject;
        if(playerDir != 0)spriteRenderer.flipX = playerDir == -1;
        if(player)
        {
            if (player.transform.position.x > transform.position.x)
            {
                playerDir = 1;
            }
            else
            {
                playerDir = -1;
            }
        }
        if (enemyHealth && enemyHealth.isDead) playerDir = 0;
        RaycastHit2D groundHit = Physics2D.CapsuleCast(transform.position, collider.size, collider.direction, 0, Vector2.down, rayLength, ground);
        if (groundHit) isGrounded = true;
        else isGrounded = false;

    }

    private void FixedUpdate()
    {
        //Movement
        if (playerDir == 0)
        {
            curVelocity.x = Mathf.MoveTowards(curVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            curVelocity.x = Mathf.MoveTowards(curVelocity.x, playerDir * maxSpeed, acceleration * Time.fixedDeltaTime);
        }

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, curVelocity.normalized, rayLength, ground);
        //if(hit.collider != null)Debug.Log(hit.collider.gameObject.name);
        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;

        if (hit && isGrounded)rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        curVelocity.y = rb.velocity.y;

        rb.velocity = curVelocity;
    }

    public int getPlayerDir()
    {
        return playerDir;
    }
}

