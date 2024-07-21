using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session:\n" + response.errorData);
                Debug.Log(response.errorData.code);

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });

        SceneManager.LoadSceneAsync("MainMenu", mode: LoadSceneMode.Additive);
    }
}
