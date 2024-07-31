using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmokeDamage : MonoBehaviour
{
    private const int initial_cooldown = 90;
    private float radius = 30;
    private int cooldown = initial_cooldown;
    private int damage = 1;
    private void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown--;
            return;
        }

        damageEnemies();
        cooldown = initial_cooldown;


    }

    private void damageEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        colliders.ToList().ForEach(collider =>
        {
            Debug.Log("Smoke cloud collider");
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Damageable damageable = collider.GetComponent<Damageable>();

                if (damageable != null)
                {
                    EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
  
                    damageable.TakeDamage(damage);
                }
            }
        });
    }

}
