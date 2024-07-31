using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShadowPotionHUD : MonoBehaviour
{
    public PotionManager potions;

    public GameObject shadowBottle;
    public GameObject emptyBottle;

    private GameObject[] shadowBottles ;
    private GameObject[] emptyBottles;

    void Start(){
      
       InitializePotions();

       potions.onNumPotionChange += UpdatePotions;
    }
 
    void InitializePotions()
    {
        shadowBottles = new GameObject[potions.maxAmmo];
        emptyBottles = new GameObject[potions.maxAmmo];
        for (int i = 0; i < potions.maxAmmo; i++)
        {
            shadowBottles[i] = Instantiate(shadowBottle, transform);
            emptyBottles[i] = Instantiate(emptyBottle, transform);
            shadowBottles[i].SetActive(i < potions.curAmmo);
            emptyBottles[i].SetActive(i >= potions.curAmmo);
        }
    }

    void UpdatePotions()
    {
        for (int i = 0; i < potions.maxAmmo; i++)
        {
            if (i < potions.curAmmo)
            {
                shadowBottles[i].SetActive(true);
                emptyBottles[i].SetActive(false);
            }
            else
            {
                shadowBottles[i].SetActive(false);
                emptyBottles[i].SetActive(true);
            }
        }
    }

}
