using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyHealth : MonoBehaviour, Damageable
{
    public int maxHealth = 5;
    public int curHealth;
    [HideInInspector]public bool isDead;
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject particalOrigin;

    [SerializeField] Animator spriteAnimator;
    [SerializeField] Collider2D damageCollider;
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
        if(damageCollider) damageCollider.enabled= false;
        StartCoroutine(destroyRoutine());
    }

    private IEnumerator destroyRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        goodBye();
    }
    private void goodBye()
    {
        if (deathEffect)
        {
            if (!particalOrigin)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            else Instantiate(deathEffect, particalOrigin.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
