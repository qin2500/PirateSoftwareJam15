using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int _wave = 0;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text waveCounter;


    private bool stopUpdating = false;


    private void Awake()
    {
        GlobalReferences.LEVELMANAGER = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //reset globals

        GlobalEvents.PlayerPause.uninvoke();
        GlobalEvents.PlayerDeath.uninvoke();
        GlobalEvents.LevelComplete.uninvoke();
        GlobalEvents.PlayerStartedMoving.uninvoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopUpdating) return;

        if (GlobalReferences.PLAYER.Exp > GlobalReferences.LEVELEXP)
        {


        }
        if (GlobalEvents.PlayerStartedMoving.Invoked() && !GlobalEvents.PlayerPause.Invoked())
        {
            //will probably signal wave to start/resume
        }

        if (GlobalEvents.PlayerDeath.Invoked())
        {
            restartLevel();
            GlobalEvents.PlayerDeath.uninvoke();
            GlobalEvents.LevelComplete.uninvoke();
            return;
        }

        if (GlobalEvents.LevelComplete.Invoked())
        {
            loadMainMenu();
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && !GlobalEvents.PlayerPause.Invoked())
        {
            if (GlobalEvents.PlayerPause.Invoked()) togglePauseMenu();
            GlobalEvents.PlayerPause.uninvoke();
            restartLevel();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GlobalEvents.PlayerPause.Invoked()) togglePauseMenu();
            GlobalEvents.PlayerPause.uninvoke();
            GlobalEvents.PlayerStartedMoving.uninvoke();
            GlobalEvents.FullPlaythroughInProgress.uninvoke();
            loadMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            //pause button
            togglePauseMenu();

        }

    }

    public void setWave(int wave)
    {

        if (wave == 0)
        {
            Debug.Log("Some bozo tried to set the wave to 0");
            throw new Exception("wave cannot be set to 0");
        }

        UnloadWave(() =>
        {
            Debug.Log("Loading wave: " + this._wave);
            SceneManager.LoadSceneAsync("wave " + this._wave, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
            {
                SceneManager.LoadSceneAsync("Player Controller", mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
                {
                    Debug.Log("Loaded wave: " + this._wave);

                    waveCounter.text = "Wave " + this._wave;
                };
            };
        });
    }

    public void UnloadWave(System.Action callback)
    {
        Debug.Log("unload wave called for wave: " + this._wave);
        if (this._wave > 0)
        {
            Debug.Log("Unloading wave: " + this._wave);
            SceneManager.UnloadSceneAsync("wave " + this._wave).completed += (asyncOperation) =>
            {
                SceneManager.UnloadSceneAsync("Player Controller").completed += (asyncOperation) =>
                {
                    callback();
                };
            };
        } else
        {
            callback();
        }
    }

    public void incrementLevel() { setWave(this._wave + 1); }

    public void restartLevel()
    {
        setWave(this._wave);
    }

    public void loadMainMenu()
    {
        UnloadWave(() => 
        { 
            SceneManager.LoadSceneAsync("MainMenu", mode: LoadSceneMode.Additive).completed+=(asyncOperation)=> 
            { 
                SceneManager.UnloadSceneAsync("LevelController");
            };
        });
    }

    private void togglePauseMenu()
    {
        if (GlobalEvents.PlayerPause.Invoked())
        {
            SceneManager.UnloadSceneAsync("PauseMenu").completed += (asyncOperation) => 
            {
                GlobalEvents.PlayerPause.uninvoke();
            };

        }
        else
        {
            SceneManager.LoadSceneAsync("PauseMenu", mode: LoadSceneMode.Additive).completed += (asyncOperation) => 
            { 
                GlobalEvents.PlayerPause.invoke();
            };
        }
    }

    private void displayUpgradeMenu()
    {
        SceneManager.LoadSceneAsync(SceneNames.UPGRADEMENU, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
        {
            GlobalEvents.PlayerPause.invoke();
        };
    }

    private string levelString()
    {
        return "level" + this._wave;
    }
}
