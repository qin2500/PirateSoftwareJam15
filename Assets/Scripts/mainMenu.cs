using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject canvas;

    public void Play()
    {
        SceneManager.LoadSceneAsync(SceneNames.LEVELCONTROLLER, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
        {
            hideAssets();
        };
        
    }

    public void loadCredits()
    {
        SceneManager.LoadSceneAsync(SceneNames.CREDITS, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
        {
            hideAssets();
        };
    }

    private void hideAssets()
    {
        canvas.SetActive(false);
    }
}
