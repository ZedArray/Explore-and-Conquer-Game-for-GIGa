using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    int enemyAmount;
    bool isPaused;

    [SerializeField] TextMeshProUGUI enemyCounter;
    [SerializeField] Image gunCooldown;
    [SerializeField] Image knifeCooldown;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deathMenu;
    [SerializeField] TextMeshProUGUI bulletCounter;
    [SerializeField] TextMeshProUGUI levelIndicator;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        levelIndicator.text = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        enemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCounter.text = "TARGETS LEFT: " + enemyAmount;

    }

    public void updateBullet(int bulletAmount, int bulletMax)
    {
        bulletCounter.text = bulletAmount + "/" + bulletMax;
    }

    public void pauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void deathScreen()
    {
        deathMenu.SetActive(true);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitToMenu()
    {
        isPaused = false;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void setGunCooldown(float shotRate, float shotTimer)
    {
        gunCooldown.fillAmount = shotTimer / shotRate;
    }

    public void setKnifeCooldown(float knifeRate, float knifeTimer)
    {
        knifeCooldown.fillAmount = knifeTimer / knifeRate;
    }

    public bool getIfPaused()
    {
        return isPaused;
    }
}
