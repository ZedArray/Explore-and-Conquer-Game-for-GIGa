using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnNew()
    {
        SceneManager.LoadScene(1);
    }

    public void OnLoad()
    {
        int loadScene = PlayerPrefs.GetInt("SavedScene", 0);
        if (loadScene == 0)
        {
            Debug.Log("No Save File");
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }

}
