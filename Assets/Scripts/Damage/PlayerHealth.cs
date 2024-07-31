using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] GameObject particleOrigin;
    [SerializeField] Animator spriteAnimator;

    [Header("OnDamage")]
    [SerializeField] private float iFrames;
    [SerializeField] private float knockbackForce;
    private bool isInvincible;
    [SerializeField] private Animator damageEffectAnimator;
    [SerializeField] CameraShakeController cameraShakeController;
    [SerializeField] float camShakeAmplitude = 2;
    [SerializeField] float camShakeDuration = 0.5f;
    public event Action onHealthChanged;
    

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        GlobalReferences.PLAYER.Health = this;
    }
    public void TakeDamage(int amount)
    {
        if (!cameraShakeController) cameraShakeController = GlobalReferences.SHAKECONTROLLER;
        if(isInvincible || playerMovement.getSwimming())
        {
            return;
        }
        curHealth -= amount;
        onHealthChanged.Invoke();
        StartCoroutine(InvincibilityCoroutine());
        if (cameraShakeController) cameraShakeController.shakeCamera(camShakeAmplitude, camShakeDuration);
        if (curHealth <= 0)
        {
            Debug.Log("Should die");
            death();
        }
        
    }
    public void heal(int amount)
    {   
        onHealthChanged.Invoke();
        curHealth = Mathf.Max(maxHealth, curHealth + amount);
    }
    public void knockBack(Transform trans)
    {
        if (!isInvincible)
        {
            Vector2 knockbackDirection = (transform.position - trans.position).normalized;
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
        Debug.Log("Player died!!");
        GlobalEvents.PlayerDeath.invoke();
        //Destroy(gameObject, deathDelay);
    }
    
    private void OnDestroy()
    {
        if (deathEffect)
        {
            if (!particleOrigin)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            else Instantiate(deathEffect, particleOrigin.transform.position, Quaternion.identity);
        }
    }


}
