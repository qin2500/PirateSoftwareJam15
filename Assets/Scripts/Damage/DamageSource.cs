using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{

    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().knockBack(transform);
            collision.gameObject.GetComponent<Damageable>().TakeDamage(damage);
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().knockBack(transform);
            collision.gameObject.GetComponent<Damageable>().TakeDamage(damage);
        }
    }
}
