using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuddleController : MonoBehaviour
{ 
    [SerializeField]private float lifeTime;
    [SerializeField] private ParticleSystem ps;

    public event Action onDie;
    // Start is called before the first frame update
    void Start()
    {
        ps.Stop();

        var main = ps.main;
        main.duration= lifeTime;

        ps.Play();

        Destroy(gameObject, lifeTime+1);
    }


    public void setLifeTime(float lifeTime)
    {
        this.lifeTime= lifeTime;
    }
    public void OnDestroy()
    {
        onDie.Invoke();
    }

}
