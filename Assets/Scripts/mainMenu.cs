using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(" Enter scene to go to "); // make sure the in the build, we have certain scenes preloaded
    }
    public void QuitGame(){
        Application.Quit();
    }
    
}
