using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwimBoundsController : MonoBehaviour
{
    public List<Collider2D> triggers = new List<Collider2D>();
    private PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shadow"))
        {
            if(!triggers.Contains(collision)) triggers.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(triggers.Contains(collision)) triggers.Remove(collision);
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
