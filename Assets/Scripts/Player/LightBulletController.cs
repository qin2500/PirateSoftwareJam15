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
    private Rigidbody2D rb;
    private float throwPower;
    private LayerMask ground;
    private int damage;
    public float radius;

    public float timeToDisable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Invoke("killBullet", timeToDisable);
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

        if (!particleOrigin)
            Instantiate(aoeParticleEffect, transform.position, Quaternion.identity);
        else Instantiate(aoeParticleEffect, particleOrigin.transform.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, 0);


        colliders.ToList().ForEach(collider =>
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.GetComponent<Damageable>().TakeDamage(damage);
                GlobalReferences.PLAYER.Pentagram.applyEffects(this);
                killBullet();
            }
        });
    }
    private void killBullet()
    {
        mainAttackController.returnToPool(gameObject);
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
