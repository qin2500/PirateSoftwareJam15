using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class PotionManager : MonoBehaviour
{
    [SerializeField] private GameObject shadowPotion;
    public int maxAmmo = 3;
    public int curAmmo = 3;
    private Queue<GameObject> shadowPotionPool;
    private Queue<GameObject> alchemyPotionPool;
    private Rigidbody2D rb;
    [SerializeField] private GameObject throwOrigin;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float shadowPuddleLife;
    private PlayerMovement playerMovement;
    private int cooldown = 10;

    public event Action onNumPotionChange;
    private void Awake()
    {
        shadowPotionPool = new Queue<GameObject>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Debug.Log("Initializing potion manager");
        for (int i = 0; i < maxAmmo; i++)
        {
            GameObject potion = Instantiate(shadowPotion);
            potion.transform.parent = transform;
            potion.GetComponent<ShadowPotionController>().setPotionManager(this);
            potion.GetComponent<ShadowPotionController>().setShadow(shadow);
            potion.GetComponent<ShadowPotionController>().playerMovement = playerMovement;
            potion.GetComponent<ShadowPotionController>().setLifeTime(shadowPuddleLife);
            potion.SetActive(false);
            shadowPotionPool.Enqueue(potion);
        }
    }

    public void FixedUpdate()
    {
        cooldown--;
    }

    private void Update()
    {
        if (cooldown > 0) return;

        cooldown = GlobalReferences.PLAYER.potionCooldown;
        
        if (Input.GetMouseButtonDown(1))
        {

            if (shadowPotionPool.Count > 0)
            {
                GameObject potion = shadowPotionPool.Dequeue();
                onNumPotionChange.Invoke();
                curAmmo = shadowPotionPool.Count;
                potion.GetComponent<ShadowPotionController>().setInitialVelocity(10 + rb.velocity.magnitude * 0.5f);
                potion.SetActive(true);

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - throwOrigin.transform.position;
                direction.z = 0; 

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

                potion.transform.position = throwOrigin.transform.position;
                potion.transform.rotation = rotation;


            }
            else
            {
                Debug.LogWarning("No potions available in the pool.");
            }
        } else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attempting to throw offensive potion");
        }
    }

    public void addToPool(GameObject potion)
    {   
        onNumPotionChange.Invoke();
        potion.SetActive(false);
        shadowPotionPool.Enqueue(potion); //TODO: Specify pool
    }
}
