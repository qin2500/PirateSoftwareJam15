using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShadowPotionHUD : MonoBehaviour
{
    public PotionManager potionManager;
    private int numOfPotions;

    public Image[] potions;
    public Sprite fullPotions;
    public Sprite emptyPotions;

    void Start(){
       numOfPotions = potionManager.maxAmmo;
    }
    void Update(){
        // if health decrease animate the destruction animation

        if (potionManager.curAmmo > numOfPotions){

            potionManager.curAmmo  = numOfPotions;
        }

        for (int i = 0; i < potions.Length; i++){
            if (i < potionManager.curAmmo ){
                potions[i].sprite = fullPotions;   
            }
            else{
                potions[i].sprite = emptyPotions;
            }

            if (i  < numOfPotions){
                potions[i].enabled = true;
            }
            else{
                potions[i].enabled = false;
            }
        }

        // if health at 1 hp animate the last one


    }
}
