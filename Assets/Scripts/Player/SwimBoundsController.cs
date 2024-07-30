using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwimBoundsController : MonoBehaviour
{
    public List<Collider2D> triggers = new List<Collider2D>();
    private PlayerMovement player;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D rb;
    public List<Collider2D> curTriggers = new List<Collider2D>();

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

    }
    void Update()
    {
        if (curTriggers.Count == 0 && player.getSwimming()) player.setSwimming(false);
        if(triggers.Count > 0 && player.getSwimming())
        {
            Vector2 position = transform.position;
            Bounds combinedBounds = CalculateCombinedBounds();

            position.x = Mathf.Clamp(position.x, combinedBounds.min.x + playerCollider.size.x, combinedBounds.max.x - playerCollider.size.x);
            //position.y = Mathf.Clamp(position.y, combinedBounds.min.y, combinedBounds.max.y);

            transform.position = position;
        }
        else if( !player.getSwimming())
        {
            triggers.Clear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Shadow") && player.getSwimming())
        {
            if(!curTriggers.Contains(collision))curTriggers.Add(collision);
            if (!triggers.Contains(collision))
            {
                triggers.Add(collision);
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Shadow") && player.getSwimming())
        {
            if (!curTriggers.Contains(collision)) curTriggers.Add(collision);
            if (!triggers.Contains(collision))
            {
                triggers.Add(collision);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shadow"))
        {
            curTriggers.Remove(collision);
        }
    }

    private Bounds CalculateCombinedBounds()
    {
        if (triggers.Count == 0)
        {
            return new Bounds(Vector3.zero, Vector3.zero);
        }
        cleanQueue();
        Bounds combinedBounds = triggers[0].bounds;
        for (int i = 1; i < triggers.Count; i++)
        {
            combinedBounds.Encapsulate(triggers[i].bounds);
        }

        return combinedBounds;
    }

    private void cleanQueue()
    {
        List<Collider2D> tempQ = new List<Collider2D>();
        for (int i = 0; i < triggers.Count; i++)
        {
            if (triggers[i] != null) tempQ.Add(triggers[i]);
        }
        triggers = tempQ;
    }
}
