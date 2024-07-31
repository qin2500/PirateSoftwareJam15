using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainAttackController : MonoBehaviour
{
    [SerializeField] private int potionPoolSize = 10;
    [SerializeField] private float throwPower;
    [SerializeField] private float timeToDisableBullet;
    [SerializeField] private GameObject lightBullet;
    [SerializeField] private GameObject bulletOrigin;
    [SerializeField] private int damage;
    private int cooldown = GlobalReferences.PLAYER.potionCooldown;

    private Queue<GameObject> potionPool;

    private void Awake()
    {
        potionPool = new Queue<GameObject>();
    }
    void Start()
    {
        for(int i=0; i<potionPoolSize; i++)
        {
            GameObject bullet = Instantiate(lightBullet);
            bullet.transform.parent = transform;

            LightBulletController bulletController = bullet.GetComponent<LightBulletController>();
            bulletController.setMainAttackController(this);
            bulletController.timeToDisable = timeToDisableBullet;

            bullet.SetActive(false);
            potionPool.Enqueue(bullet);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
            return;
        
        cooldown = GlobalReferences.PLAYER.potionCooldown;

        if (Input.GetMouseButtonDown(0))
        {
            int numToThrow = GlobalReferences.PLAYER.Pentagram.getNumCombinations();
            //DEBUGGING
            //numToThrow = 2;
            if (numToThrow == 0) numToThrow = 1;
            for (int i = 0; i < numToThrow; i++)
            {
                if (potionPool.Count > 0)
                {
                    GameObject bullet = potionPool.Dequeue();

                    bullet.transform.parent = transform;
                    bullet.transform.position = bulletOrigin.transform.position;
                    bullet.GetComponent<LightBulletController>().setThrowPower(throwPower);
                    bullet.GetComponent<LightBulletController>().setDamage(damage);

                    bullet.SetActive(true);

                    //Calulate Direction vector to mouse
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 direction = mousePosition - bulletOrigin.transform.position;
                    direction.z = 0;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

                    bullet.transform.position = bulletOrigin.transform.position;
                    bullet.transform.rotation = rotation;

                    bullet.transform.position += Vector3.up*1*i;

                }
                else
                {
                    Debug.LogWarning("No grenades available in the pool.");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown--;
            return;
        }
    }
    public void returnToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        potionPool.Enqueue(bullet);
    }
}
