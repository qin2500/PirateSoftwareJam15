using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    [SerializeField] private GameObject shadowGrenade;
    private Queue<GameObject> grenadePool;
    [SerializeField]private float grenadeSpeed;
    [SerializeField] private GameObject throwOrigin;
    [SerializeField] private GameObject shadow;

    private void Awake()
    {
        grenadePool = new Queue<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < 10 ; i++)
        {
            GameObject goo = Instantiate(shadowGrenade);
            goo.transform.parent = transform;
            goo.GetComponent<ShadowGernadeController>().setGrenadeManager(this);
            goo.GetComponent<ShadowGernadeController>().setShadow(shadow);
            goo.SetActive(false);
            grenadePool.Enqueue(goo);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right mouse button clicked");

            if (grenadePool.Count > 0)
            {
                GameObject goo = grenadePool.Dequeue();
                goo.GetComponent<ShadowGernadeController>().setInitialVelocity(grenadeSpeed);
                
                goo.SetActive(true);

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - throwOrigin.transform.position;
                direction.z = 0; 

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

                goo.transform.position = throwOrigin.transform.position;
                goo.transform.rotation = rotation;

                

            }
            else
            {
                Debug.LogWarning("No grenades available in the pool.");
            }
        }
    }

    public void addToPool(GameObject grenade)
    {
        grenade.SetActive(false);
        grenadePool.Enqueue(grenade);
    }
}
