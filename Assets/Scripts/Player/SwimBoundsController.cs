using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwimBoundsController : MonoBehaviour
{
    public List<Collider2D> triggers = new List<Collider2D>();
    private PlayerMovement player;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        if(triggers.Count > 0 && player.getSwimming())
        {
            Vector2 position = transform.position;
            Bounds combinedBounds = CalculateCombinedBounds();

            position.x = Mathf.Clamp(position.x, combinedBounds.min.x, combinedBounds.max.x);
            //position.y = Mathf.Clamp(position.y, combinedBounds.min.y, combinedBounds.max.y);

            transform.position = position;

            if (transform.position.x < combinedBounds.min.x && rb.velocity.x < 0) rb.velocity = new Vector2(0, rb.velocity.y);
            else if (transform.position.x > combinedBounds.max.x && rb.velocity.x > 0) rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if(triggers.Count > 0 && !player.getSwimming())
        {
            triggers.Clear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Shadow") && player.getSwimming())
        {
            if (!triggers.Contains(collision)) triggers.Add(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Shadow") && player.getSwimming())
        {
            if (!triggers.Contains(collision)) triggers.Add(collision);
        }
    }

    private Bounds CalculateCombinedBounds()
    {
        if (triggers.Count == 0)
        {
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        Bounds combinedBounds = triggers[0].bounds;
        for (int i = 1; i < triggers.Count; i++)
        {
            combinedBounds.Encapsulate(triggers[i].bounds);
        }

        return combinedBounds;
    }
}
