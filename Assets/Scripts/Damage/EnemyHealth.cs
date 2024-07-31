using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyHealth : MonoBehaviour, Damageable
{
    private Rigidbody2D enemyRb;
    public int maxHealth = 5;
    public int curHealth;
    public int exp = 0; //letting the individual enemy script be in charge of settin gthis
    public float knockbackForce = 0;
    [HideInInspector]public bool isDead;
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject particleOrigin;
    [SerializeField] Animator spriteAnimator;
    [SerializeField] Collider2D damageCollider;
    public void Awake()
    {
        curHealth = maxHealth;
        enemyRb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;

        if(curHealth <= 0 && !isDead)
        {
            death();
        }
    }
    public void death()
    {
        isDead = true; //technically race conditions here but not worth worrying about
        if (spriteAnimator)spriteAnimator.Play("Death");
        if(damageCollider) damageCollider.enabled= false;
        GlobalReferences.PLAYER.Exp += exp;
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
            if (!particleOrigin)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            else Instantiate(deathEffect, particleOrigin.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public void knockBack(Transform transf)
    {
            Vector2 knockbackDirection = (transform.position - transf.position).normalized;
            knockbackDirection.y += 2;
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    public void setExp(int exp)
    {
        this.exp = exp;
    }
}
