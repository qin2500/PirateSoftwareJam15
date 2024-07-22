using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBoxController : MonoBehaviour
{
    [SerializeField] private GameObject ghostWalls;
    private PlayerMovement playerMovement;

    public void activateGhostWalls()
    {
        ghostWalls.SetActive(true);
    }
    public void deactivateGhostwalls()
    {
        ghostWalls.SetActive(false);
    }
    public void setPlayerMovement(PlayerMovement movement)
    {
        this.playerMovement = movement;

    }
    private void Start()
    {
        playerMovement.onPlayerSwim.AddListener( activateGhostWalls);
        playerMovement.onPlayerSurface.AddListener(deactivateGhostwalls);
    }
}
