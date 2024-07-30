using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Damageable
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    [Header("Health")]
    public int maxHealth = 5;
    public int curHealth;
    [HideInInspector] public bool isDead;

    [Header("Death")]
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject particalOrigin;
    [SerializeField] Animator spriteAnimator;

    [Header("OnDamage")]
    [SerializeField] private float iFrames;
    [SerializeField] private float knockbackForce;
    private bool isInvincible;
    [SerializeField] private Animator damageEffectAnimator;
    [SerializeField] CameraShakeController cameraShakeController;
    [SerializeField] float camShakeAmplitude = 2;
    [SerializeField] float camShakeDuration = 0.5f;
    public event Action onDamage;

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(int amount)
    {
        if(isInvincible || playerMovement.getSwimming())
        {
            return;
        }
        curHealth -= amount;
        StartCoroutine(InvincibilityCoroutine());
        cameraShakeController.shakeCamera(camShakeAmplitude, camShakeDuration   );
        if (curHealth <= 0)
        {
            death();
        }
    }
    public void heal(int amount)
    {
        curHealth = Mathf.Max(maxHealth, curHealth + amount);
    }
    public void knockBack(Transform trans)
    {
        if (!isInvincible)
        {
            Vector2 knockbackDirection = (transform.position - trans.position).normalized;
            Debug.Log(knockbackDirection);
            knockbackDirection.y += 2;
            //rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            playerMovement.setHitStunVelocity((knockbackDirection.normalized * knockbackForce));
            playerMovement.setHitStun(true);
        }
    }
    


    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        if (damageEffectAnimator != null)
        {
            damageEffectAnimator.Play("Invincible");
        }
        yield return new WaitForSeconds(iFrames);
        damageEffectAnimator.Play("Nothing");
        isInvincible = false;
    }

    public void death()
    {
        if (spriteAnimator) spriteAnimator.Play("Death");
        isDead = true;
        Debug.Log("YOU ARE DIE!!");
        //Destroy(gameObject, deathDelay);
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
