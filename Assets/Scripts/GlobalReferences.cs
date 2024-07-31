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
    private static CameraShakeController shakeController;

    public static GameManager GAMEMANAGER {  get { return gameManager; } set { gameManager = value; } }
    public static LevelManager LEVELMANAGER { get { return levelManager; } set { levelManager = value; } }
    //public static SoundMixerManager SOUNDMIXERMANAGER { get {return soundMixerManager; } set {soundMixerManager = value;}}
    public static GameObject STARTINDICATOR { get { return startIndicator; } set { startIndicator = value; } }
    public static Player PLAYER { get { return player; } set { player = value; } }

    public static CameraShakeController SHAKECONTROLLER { get { return shakeController; }  set { shakeController = value; } }

    public const int LEVELEXP = 100;
}

public class Player
{
    private string username;
    private int score;
    private int exp;
    private int level;
    private Pentagram pentagram = new Pentagram();
    private PlayerHealth health;
    private GameObject playerObject;
    public string Username { get { return username; } set { username = value; } }
    public int Score { get { return score; } set { score = value; } }
    public Vector2 startPosition = new Vector2(-2.62f, 3.55f);
    public int potionCooldown = 0;
    public int Level { get { return level; } set { level = value; } }
    public int Exp { get { return exp; } set { exp = value; } }

    public Pentagram Pentagram { get { return pentagram; } set { pentagram = value; } }
    public PlayerHealth Health { get { return health;   } set { health = value; } } 

    public GameObject PlayerObject { get {  return playerObject; } set { playerObject = value; } }   

    public void resetPlayer()
    {
        this.exp = 0;
        this.level = 0;
    }
}


public static class SceneNames
{
    public const string MAINMENU = "MainMenu";
    public const string PLAYERCONTROLLER = "PlayerController";
    public const string CREDITS = "Credits";
    public const string PAUSEMENU = "PauseMenu";
    public const string LEVELCONTROLLER = "LevelController";
    public const string GAMEMANAGER = "GameManager";
    public const string UPGRADEMENU = "UpgradeMenu";
    public const string COMBINEMENU = "CombineMenu";
    public const string ARENA = "PlayerMovement"; //TODO: PLEASE CHANGE THIS NAME
    public const string TUTORIAL = "Tutorial";
}
