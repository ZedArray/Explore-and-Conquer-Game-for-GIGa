using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchBox : MonoBehaviour
{
    bool hitonce = false;
    [SerializeField] int nextScene;
    private void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        int enemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && !hitonce && enemyAmount == 0)
        {
            PlayerPrefs.SetInt("SavedScene", nextScene);
            SceneManager.LoadScene(nextScene);
            hitonce = true;
        }
        else if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && !hitonce && enemyAmount != 0)
        {
            PlayerPrefs.SetInt("SavedScene", nextScene);
            SceneManager.LoadScene(nextScene);
            hitonce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitonce = false;
    }

}
