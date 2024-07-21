using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public static class GlobalReferences
{
    private static GameManager gameManager;
    private static LevelManager levelManager;
    private static GameObject startIndicator;
    //private static SoundMixerManager soundMixerManager;
    private static Player player = new();

    public static GameManager GAMEMANAGER {  get { return gameManager; } set { gameManager = value; } }
    public static LevelManager LEVELMANAGER { get { return levelManager; } set { levelManager = value; } }
    //public static SoundMixerManager SOUNDMIXERMANAGER { get {return soundMixerManager; } set {soundMixerManager = value;}}
    public static GameObject STARTINDICATOR { get { return startIndicator; } set { startIndicator = value; } }
    public static Player PLAYER { get { return player; } set { player = value; } }

    public const int NUMBEROFLEVELS = 5;
    public static int initalLeaderboardIndex = 0;
}

public class Player
{
    private string username;
    private int score;
    public string Username { get { return username; } set { username = value; } }
    public int Score { get { return score; } set { score = value; } }
    public Vector2 startPosition = new Vector2(-2.62f, 3.55f);
}

public class AlchemyUpgrade
{
    private HashSet<>
    public override string ToString()
    {
        return base.ToString();
    }
} 

public class Pentagram
{
    private AlchemyUpgrade[] upgrades;

}

public static class SceneNames
{
    public const string MAINMENU = "MainMenu";
    public const string LEVELSELECTOR = "LevelSelector";
    public const string PLAYERCONTROLLER = "Player Controller";
    public const string CREDITS = "Credits";
    public const string PAUSEMENU = "PauseMenu";
    public const string LEVELCONTROLLER = "LevelController";
    public const string NAMESELECTOR = "NameSelector";
    public const string GAMEMANAGER = "GameManager";
    public const string LEADERBOARD = "Leaderboard";
}
