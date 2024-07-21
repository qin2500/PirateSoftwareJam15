using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGernadeController : MonoBehaviour
{
    private Rigidbody2D rb;
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
    }

    public void setInitialVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }
}
