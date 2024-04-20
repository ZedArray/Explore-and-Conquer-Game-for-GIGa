using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Weapons weapons;
    [SerializeField] UIScript ui;

    float shotRate;
    float shotTimer;
    bool isDead;
    public int bulletAmount;
    public int bulletMax;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        shotRate = weapons.getShotRate();
        shotTimer = weapons.getShotTimer();
        ui.setGunCooldown(shotRate, shotTimer);
        ui.setKnifeCooldown(weapons.getKnifeRate(), weapons.getKnifeCTimer());

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ui.pauseGame();
        }

        bulletAmount = weapons.getBulletAmount();
        bulletMax = weapons.getBulletMax();
        ui.updateBullet(bulletAmount, bulletMax);
    }

    public void setBullets()
    {
        weapons.setBullets(PlayerPrefs.GetInt("BulletAmount"), PlayerPrefs.GetInt("BulletMax"));
    }

    public void gameOver()
    {
        isDead = true;
        StartCoroutine(DeathScreen());
    }

    public bool getIsDead()
    {
        return isDead;
    }

    IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(2f);
        ui.deathScreen();
    }

    public bool getIfPaused()
    {
        return ui.getIfPaused();
    }
}
