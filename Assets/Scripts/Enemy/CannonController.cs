using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private MainAttackController mainAttackController;
    [SerializeField] private GameObject aoeParticleEffect;
    [SerializeField] private GameObject particleOrigin;
    [SerializeField] private GameObject healingPotion;
    private Rigidbody2D rb;
    private float throwPower;
    private LayerMask ground;
    [SerializeField] public float radius;
    public float knockbackForce = 0;
    private Quaternion initialRotation;

    public bool burnEnemies = false;
    public bool chainBounce = false;
    public bool healOnHit = false;
    public bool smokeOnDeath = false;
    public bool explodeOnDeath = false;
    public int slowFrames = 0;

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
        Destroy(gameObject, 0.01f);

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
