using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator playerAnimator;

    [SerializeField] private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator= GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 curVelocity = playerMovement.getCurVelocity();
        if (curVelocity.x != 0) sprite.flipX = curVelocity.x < 0;

        if (playerMovement.getIsGrounded())
        {
            if(curVelocity.x != 0)
            {
                playerAnimator.Play("Run");
            }
            else
            {
                playerAnimator.Play("Idle");
            }
        }
        else
        {
            if(curVelocity.y > 0)
            {
                playerAnimator.Play("Jump");
            }
            else if(curVelocity.y < 0)
            {
                playerAnimator.Play("Fall");
            }
        }
    }
}
