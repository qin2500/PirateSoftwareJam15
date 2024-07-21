using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int _level = 0;
    private bool _timerActive = false;
    private float _currentTime;
    private int deaths = 0;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text deathCounter;
    [SerializeField] private TMP_Text levelCounter;


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
        if (GlobalEvents.PlayerStartedMoving.Invoked() && !GlobalEvents.PlayerPause.Invoked())
        {
            _timerActive = true;
        }

        if (GlobalEvents.PlayerDeath.Invoked())
        {
            deathCounter.text = "Deaths: " + ++deaths;
            restartLevel();
            GlobalEvents.PlayerDeath.uninvoke();
            GlobalEvents.LevelComplete.uninvoke();
            return;
        }

        if (GlobalEvents.LevelComplete.Invoked())
        {
            completeLevel();
            GlobalEvents.LevelComplete.uninvoke();
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

        if (_timerActive)
        {
            _currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        _text.text = time.Minutes + ":" + time.Seconds + ":" + time.Milliseconds;

        
    }

    public void setLevel(int level)
    {

        if (level == 0)
        {
            Debug.Log("Some bozo tried to set the level to 0");
            throw new Exception("level cannot be set to 0");
        }

        UnloadLevel(() =>
        {
            this._level = level;
            Debug.Log("Loading level: " + this._level);
            SceneManager.LoadSceneAsync("Level " + this._level, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
            {
                SceneManager.LoadSceneAsync("Player Controller", mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
                {
                    Debug.Log("Loaded level: " + this._level);

                    if (!GlobalEvents.FullPlaythroughInProgress.Invoked()) resetTimer();

                    unpauseTimer();

                    levelCounter.text = "Level " + this._level;
                };
            };
        });
    }

    public void UnloadLevel(System.Action callback)
    {
        Debug.Log("unload level called for level: " + this._level);
        if (this._level > 0)
        {
            Debug.Log("Unloading level: " + this._level);
            SceneManager.UnloadSceneAsync("Level " + this._level).completed += (asyncOperation) =>
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

    public void incrementLevel() { setLevel(this._level + 1); }

    public void restartLevel()
    {
        setLevel(this._level);
    }

    public void completeLevel() 
    {
        //stop counting time and turn it into score then add to leaderboard

        
        
        pauseTimer();
        int score = LeaderBoardGateway.convertTimestampToScore(_currentTime);

        Debug.Log("Level completed, score is: " + score);

        // if single level then add to leaderboard for level
        if (!GlobalEvents.FullPlaythroughInProgress.Invoked())
        {
            LeaderBoardGateway.SubmitScore(levelString(), GlobalReferences.PLAYER.Username, score);
            GlobalReferences.initalLeaderboardIndex = this._level;
            stopUpdating = true;
            loadLeaderboard();
            return;
        }

        //if full playthrough add to overall leaderboard

        if (this._level == GlobalReferences.NUMBEROFLEVELS)
        {
            GlobalReferences.PLAYER.Score += score;
            LeaderBoardGateway.SubmitScore("AnyPercent", GlobalReferences.PLAYER.Username, GlobalReferences.PLAYER.Score);
            GlobalReferences.initalLeaderboardIndex = 0;
            stopUpdating = true;
            loadLeaderboard();
            return;
        }
        incrementLevel(); 
    }

    public void pauseTimer() { _timerActive = false; }
    public void unpauseTimer() {  _timerActive = true; }

    public void resetTimer()
    {
        _currentTime = 0;
        GlobalEvents.PlayerStartedMoving.uninvoke();
    }

    public bool isTimerPaused() { return !_timerActive; }

    public void loadMainMenu()
    {
        UnloadLevel(() => 
        { 
            SceneManager.LoadSceneAsync("MainMenu", mode: LoadSceneMode.Additive).completed+=(asyncOperation)=> 
            { 
                SceneManager.UnloadSceneAsync("LevelController");
            };
        });
    }


    public void loadLeaderboard()
    {
        GlobalEvents.PlayerPause.invoke();
        SceneManager.LoadSceneAsync(SceneNames.LEADERBOARD, mode: LoadSceneMode.Additive);
    }

    private void togglePauseMenu()
    {
        if (GlobalEvents.PlayerPause.Invoked())
        {
            SceneManager.UnloadSceneAsync("PauseMenu").completed += (asyncOperation) => 
            {
                if (GlobalEvents.PlayerStartedMoving.Invoked())
                    unpauseTimer();
                GlobalEvents.PlayerPause.uninvoke();
            };

        }
        else
        {
            pauseTimer();
            SceneManager.LoadSceneAsync("PauseMenu", mode: LoadSceneMode.Additive).completed += (asyncOperation) => 
            { 
                GlobalEvents.PlayerPause.invoke();
            };
        }
    }

    private string levelString()
    {
        return "level" + this._level;
    }
}
