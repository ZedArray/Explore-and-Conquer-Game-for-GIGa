using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Toggle skiptutorial;
    public void OnNew()
    {
        if (!skiptutorial.isOn)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
        PlayerPrefs.SetInt("BulletAmount", 7);
        PlayerPrefs.SetInt("BulletMax", 14);
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
