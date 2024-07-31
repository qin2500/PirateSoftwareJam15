using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyHealth : MonoBehaviour, Damageable
{
    public int maxHealth = 5;
    public int curHealth;
    public int exp = 0; //letting the individual enemy script be in charge of settin gthis
    [HideInInspector]public bool isDead;
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject particleOrigin;

    [SerializeField] Animator spriteAnimator;
    [SerializeField] Collider2D damageCollider;
    [SerializeField] ParticleSystem particleSystem;
    public void Awake()
    {
        curHealth = maxHealth;
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        particleSystem.Play();

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

    public void setExp(int exp)
    {
        this.exp = exp;
    }
}
