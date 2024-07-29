using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour, Damageable
{
    public int maxHealth = 5;
    public int curHealth;
    [HideInInspector]public bool isDead;
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject particalOrigin;

    [SerializeField] Animator spriteAnimator;
    public void Awake()
    {
        curHealth = maxHealth;
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;

        if(curHealth <= 0)
        {
            death();
        }
    }
    public void death()
    {
        if(spriteAnimator)spriteAnimator.Play("Death");
        isDead= true;
        Destroy(gameObject, deathDelay);
    }
    private void OnDestroy()
    {
        if (deathEffect)
        {
            if (!particalOrigin)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            else Instantiate(deathEffect, particalOrigin.transform.position, Quaternion.identity);
        }
    }
}
