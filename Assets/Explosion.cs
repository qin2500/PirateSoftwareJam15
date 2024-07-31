using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float radius = 30;
    private int damage = 3;
    private void Start()
    {


        damageEnemies();


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
