using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHearts : MonoBehaviour
{
    public PlayerHealth health;
    private int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start(){
       numOfHearts = health.maxHealth;
    }
    void Update(){
        if (health.curHealth > numOfHearts){
            health.curHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++){
            if (i < health.curHealth ){
                hearts[i].sprite = fullHeart;   
            }
            else{
                hearts[i].sprite = emptyHeart;
            }

            if (i  < numOfHearts){
                hearts[i].enabled = true;
            }
            else{
                hearts[i].enabled = false;
            }
        }


    }

}
