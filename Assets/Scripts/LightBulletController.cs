using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightBulletController : MonoBehaviour
{
    private MainAttackController mainAttackController;
    private Rigidbody2D rb;
    private float throwPower;
    private LayerMask ground;

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
        if (((1 << collision.gameObject.layer) & ground) != 0)
        {
            mainAttackController.returnToPool(gameObject);
        }
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
}
