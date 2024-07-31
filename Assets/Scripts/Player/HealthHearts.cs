using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthHearts : MonoBehaviour
{
    public PlayerHealth health;

    public GameObject heart;
    public GameObject emptyHeart;
    private Animator animator; 
    private int tick;

    private GameObject[] hearts ;
    private GameObject[] emptyHearts;

    void Start(){
       animator = GetComponent<Animator>();
       InitializeHearts();
       tick = 0;
       health.onHealthChanged += UpdateHearts;
    }
    void Update(){

        if (tick == 666){
            tick = 0;
        }
        tick++;

    }

 
    void InitializeHearts()
    {
        hearts = new GameObject[health.maxHealth];
        emptyHearts = new GameObject[health.maxHealth];
        for (int i = 0; i < health.maxHealth; i++)
        {
            hearts[i] = Instantiate(heart, transform);
            emptyHearts[i] = Instantiate(emptyHeart, transform);
            hearts[i].SetActive(i < health.curHealth);
            emptyHearts[i].SetActive(i >= health.curHealth);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < health.maxHealth; i++)
        {
            if (i < health.curHealth)
            {
                hearts[i].SetActive(true);
                emptyHearts[i].SetActive(false);
            }
            else
            {
                hearts[i].SetActive(false);
                emptyHearts[i].SetActive(true);
            }
        }
    }

}
