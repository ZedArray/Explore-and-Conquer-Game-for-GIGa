using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchBox : MonoBehaviour
{
    bool hitonce = false;
    int enemyAmount;
    int sceneCount;
    [SerializeField] GameManager gm;
    [SerializeField] int nextScene;
    [SerializeField] SpriteRenderer sr;

    private void Awake()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    private void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene > sceneCount - 1)
        {
            nextScene = 0;
        }
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        enemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyAmount > 0)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.green;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && !hitonce && enemyAmount == 0)
        {
            PlayerPrefs.SetInt("SavedScene", nextScene);
            SceneManager.LoadScene(nextScene);
            PlayerPrefs.SetInt("BulletAmount", gm.bulletAmount);
            PlayerPrefs.SetInt("BulletMax", gm.bulletMax);
            hitonce = true;
        }
        else if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && !hitonce && enemyAmount != 0)
        {
            hitonce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitonce = false;
    }

}
