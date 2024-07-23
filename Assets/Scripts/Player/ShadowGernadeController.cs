using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowGernadeController : MonoBehaviour
{
    private Rigidbody2D rb;
    public LayerMask ground;
    private GrenadeManager manager;
    private GameObject shadow;
    public PlayerMovement playerMovement;
    [SerializeField] private float velocity = 500f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(ApplyInitialForce());
    }

    private IEnumerator ApplyInitialForce()
    {
        yield return null; 
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * velocity, ForceMode2D.Impulse);
        rb.AddForce(Vector2.up * velocity * 0.5f, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Spawn Shadow pool here
        if (((1 << collision.gameObject.layer) & ground) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                float angle = Vector2.Angle(normal, Vector2.up);

                //Top Colliision
                if (angle < 45)
                {
                    GameObject spawnedShadow = Instantiate(shadow);
                    spawnedShadow.transform.position = contact.point;

                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    manager.addToPool(gameObject);
                }
                //Bottem Collision
                else if (angle > 135)
                {
                    Debug.Log("Bottom collision");
                    

                }
                

            }

        }
    }

    public void setInitialVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }

    public void setGrenadeManager(GrenadeManager manager)
    {
        this.manager = manager;
    }
    public void setShadow(GameObject shadow)
    {
        this.shadow= shadow;
    }

}
