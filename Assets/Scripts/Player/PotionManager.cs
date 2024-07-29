using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    [SerializeField] private GameObject shadowPotion;
    private Queue<GameObject> shadowPotionPool;
    private Queue<GameObject> alchemyPotionPool;
    private Rigidbody2D rb;
    [SerializeField] private GameObject throwOrigin;
    private int cooldown = 10;
    private const int defaultCooldown = 10;

    private void Awake()
    {
        shadowPotionPool = new Queue<GameObject>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Debug.Log("Initializing potion manager");
        for (int i = 0; i < 20 ; i++)
        {
            GameObject potion = Instantiate(shadowPotion);
            potion.transform.parent = transform;
            potion.SetActive(false);
            shadowPotionPool.Enqueue(potion);
        }
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown--;
            return;
        }
        cooldown = GlobalReferences.PLAYER.potionCooldown;
        
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right mouse button clicked");

            if (shadowPotionPool.Count > 0)
            {
                GameObject potion = shadowPotionPool.Dequeue();
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

    public void ReturnPotionToPool(GameObject potion)
    {
        potion.SetActive(false);
        shadowPotionPool.Enqueue(potion);
    }
}
