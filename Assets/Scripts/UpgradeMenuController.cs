using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeMenuController : MonoBehaviour
{

    void addFireElement()
    {
        GlobalReferences.PLAYER.Pentagram.addUpgrade(AlchemyUpgrade.from(Element.Fire, null));
        returnToGame();
    }

    void addWaterElement()
    {
        GlobalReferences.PLAYER.Pentagram.addUpgrade(AlchemyUpgrade.from(Element.Water, null));
        returnToGame();
    }

    void addGrassElement()
    {
        GlobalReferences.PLAYER.Pentagram.addUpgrade(AlchemyUpgrade.from(Element.Grass, null));
        returnToGame();
    }

    private void returnToGame()
    {
        SceneManager.UnloadSceneAsync(SceneNames.UPGRADEMENU).completed += (asyncOperation) =>
        {
            GlobalEvents.PlayerPause.uninvoke();
        };
    }
}
