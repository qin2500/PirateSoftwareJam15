using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightBulletController : MonoBehaviour
{
    private MainAttackController mainAttackController;
    
    [SerializeField] private GameObject aoeParticleEffect;
    [SerializeField] private GameObject particleOrigin;
    [SerializeField] private GameObject healingPotion;
    private Rigidbody2D rb;
    private float throwPower;
    private LayerMask ground;
    private int damage;
    [SerializeField] public float radius;
    public float knockbackForce = 0;
    private Quaternion initialRotation;

    public bool burnEnemies = false;
    public bool chainBounce = false;
    public bool healOnHit = false;
    public bool smokeOnDeath = false;
    public bool explodeOnDeath = false;
    public int slowFrames= 0;

    public float timeToDisable;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Invoke("killBullet", timeToDisable);
        initialRotation = transform.rotation;
        StartCoroutine(ApplyInitialForce());
    }

    private IEnumerator ApplyInitialForce()
    {
        yield return null;
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * throwPower, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundFXManager.instance.PlaySoundFXClip(mainAttackController.lightPotionCrash, transform, 1f);

        if (!particleOrigin)
            Instantiate(aoeParticleEffect, transform.position, Quaternion.identity);
        else Instantiate(aoeParticleEffect, particleOrigin.transform.position, Quaternion.identity);
        GlobalReferences.PLAYER.Pentagram.applyEffects(this);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);


        if (!chainBounce) killBullet();
        else bounceBullet();

        colliders.ToList().ForEach(collider =>
        {
            Debug.Log("Attack potion collider");
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Damageable damageable = collider.GetComponent<Damageable>();

                if (damageable != null)
                {
                    EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                    enemyHealth.knockbackForce = knockbackForce;
                    enemyHealth.smokeOnDeath = smokeOnDeath;
                    enemyHealth.explodeOnDeath = explodeOnDeath;
                    enemyHealth.slowFrames = slowFrames;

                    damageable.TakeDamage(damage);
                    if (healOnHit) dropHeal();
                }
            }
        });

    }
    private void killBullet()
    {
        mainAttackController.returnToPool(gameObject);
    }

    private void bounceBullet()
    {
        transform.rotation = initialRotation;
    }

    private void dropHeal()
    {
        Instantiate(healingPotion, transform.position, Quaternion.identity);
    }


    public void setThrowPower(float power)
    {
        this.throwPower = power;
    }
    public void setMainAttackController(MainAttackController controller)
    {
        mainAttackController = controller;
    }
    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
