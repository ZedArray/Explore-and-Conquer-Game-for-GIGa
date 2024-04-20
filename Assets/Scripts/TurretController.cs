using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip gunShotSFX;
    [SerializeField] Transform shotPoint;

    [SerializeField] GameObject bulletPrefab;

    float bulletSpeed = 15f;
    float guntimer;
    int bulletCounter;

    // Start is called before the first frame update
    void Start()
    {
        guntimer = 0;
        bulletCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        guntimer += Time.deltaTime;
        if (bulletCounter < 6)
        {
            if (guntimer > 5.2f)
            {
                shoot();
                guntimer = 5;
                bulletCounter++;
            }
        }
        else if (bulletCounter == 6)
        {
            if (guntimer > 10f)
            {
                guntimer = 5;
                bulletCounter = 0;
            }
        }
    }

    private void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shotPoint.right * bulletSpeed, ForceMode2D.Impulse);
        audioPlayer.clip = gunShotSFX;
        audioPlayer.Play();
    }
}
