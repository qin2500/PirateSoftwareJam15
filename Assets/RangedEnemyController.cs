using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    public GameObject muzzle;
    public float fireRate;
    public float bulletSpeed;
    public GameObject cannonBall;

    private float fireAC;
    private GroundedFollowPlayer follow;
    // Start is called before the first frame update
    void Start()
    {
        follow= GetComponent<GroundedFollowPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fireAC >= fireRate)
        {
            if (follow.getPlayerDir() > 0)
            {
                Instantiate(cannonBall, muzzle.transform.position,muzzle.transform.rotation);
            }
        }
    }
}
