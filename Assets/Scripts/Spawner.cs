using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private const int INITIAL_COOLDOWN = 360;
    private int cooldown = INITIAL_COOLDOWN;

    private void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown--;
            return;
        }
        cooldown = INITIAL_COOLDOWN;

        spawnEnemy();

    }

    private void spawnEnemy()
    {
        if (GlobalReferences.LEVELMANAGER.enemyCredits <= 0) return; //do not spawn past threshold
        GlobalReferences.LEVELMANAGER.enemyCredits--;
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
}
