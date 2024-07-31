using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    [SerializeField] GameObject burnParticleEffect;
    [SerializeField] GameObject smokeCloud;
    [SerializeField] GameObject explosion;
    public int burnTicker = 0;
    public bool smokeOnDeath = false;
    public bool explodeOnDeath = false;
    public int slowFrames = 0;
    public void Awake()
    {
        curHealth = maxHealth;
        enemyRb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        //apply burn damage
        if (burnTicker <= 0) return;
        
        if (burnTicker % 30 == 0)  TakeDamage(1);

        if (burnParticleEffect)
        {
            if (!particleOrigin)
                Instantiate(burnParticleEffect, transform.position, Quaternion.identity);
            else Instantiate(burnParticleEffect, particleOrigin.transform.position, Quaternion.identity);
        }

        burnTicker--;

    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;

        if(curHealth <= 0 && !isDead)
        {
            death();
        }

        smokeOnDeath = false;
    }
    public void death()
    {
        isDead = true; //technically race conditions here but not worth worrying about
        if (spriteAnimator)spriteAnimator.Play("Death");
        if(damageCollider) damageCollider.enabled= false;
        GlobalReferences.PLAYER.Exp += exp;
        if (smokeOnDeath) spawnSmokeCloud();
        if (explodeOnDeath) spawnExplosion();
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

    private void spawnSmokeCloud()
    {
        Instantiate(smokeCloud, transform.position, Quaternion.identity);
    }


    private void spawnExplosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
