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

    private Rigidbody2D rb;
    private Vector2 curVelocity;
    private int playerDir = 0;
    private EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth= GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
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

        curVelocity.y = rb.velocity.y;

        rb.velocity = curVelocity;
    }
}
