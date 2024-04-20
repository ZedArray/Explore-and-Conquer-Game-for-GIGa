using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shotPoint;
    [SerializeField] Collider2D knifeHitBox;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip gunShotSFX, knifeSwingSFX, emptyGunShotSFX, reloadSFX;
    [SerializeField] Animator knifeSwing;
    [SerializeField] bool unlimitedAmmo;

    bool isReloading;
    bool isKnifing;
    bool isShooting;
    float knifeCooldownTimer;
    float knifeCooldown = 1;
    float shotTimer;
    float shotRate;
    int bulletMax = 14;
    int bulletAmount = 7;

    public float bulletSpeed = 30f;
    // Start is called before the first frame update

    private void Awake()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
        knifeSwing.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    void Start()
    {
        isKnifing = false;
        isReloading = false;
        isShooting = false;
        knifeCooldownTimer = 0;
        shotTimer = 0.5f;
        shotRate = 0.7f;
        bulletAmount = PlayerPrefs.GetInt("BulletAmount");
        bulletMax = PlayerPrefs.GetInt("BulletMax");
        if (unlimitedAmmo)
        {
            bulletAmount = 7;
            bulletMax = 99;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.getIfPaused() || gm.getIsDead())
        {
            return;
        }
        if (unlimitedAmmo)
        {
            bulletMax = 99;
        }
        shotTimer += Time.deltaTime;
        knifeCooldownTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shotTimer >= shotRate && bulletAmount > 0 && !isReloading)
        {
            GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shotPoint.right * bulletSpeed, ForceMode2D.Impulse);
            shotTimer = 0f;
            src.clip = gunShotSFX;
            src.Play();
            bulletAmount--;
        }
        else if (Input.GetMouseButtonDown(0) && shotTimer >= shotRate && bulletAmount <= 0 && !isReloading)
        {
            src.clip = emptyGunShotSFX;
            src.Play();
            shotTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletAmount < 7 && bulletMax > 0 && !isReloading)
        {
            src.clip = reloadSFX;
            src.Play();
            reloadGun();
        }

        if (shotTimer == 0f)
        {
            isShooting = true;
        }
        else if (shotTimer >= 0.1f)
        {
            isShooting = false;
        }

        if (Input.GetMouseButtonDown(1) && knifeCooldownTimer >= knifeCooldown && !isReloading)
        {
            knifeSwing.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(KnifeAttack());
            knifeSwing.SetTrigger("knifeSwing");
            knifeCooldownTimer = 0;
            src.clip = knifeSwingSFX;
            src.Play();
        }

        /*if (isKnifing)
        {
            knifeTimer += Time.deltaTime;
        }*/

        /*if (Input.GetMouseButtonDown(1))
        {
            knifeSwing.SetTrigger("knifeSwing");
        }*/

        /*if (Input.GetMouseButtonDown(1) && knifeCooldownTimer >= knifeCooldown)
        {
            if (knifeTimer > 0.2f)
            {
                isKnifing = false;
            }
            else
            {
                src.clip = knifeSwingSFX;
                src.Play();
                isKnifing = true;
                knifeTimer += Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isKnifing = false;
            knifeTimer = 0;
            if (knifeCooldownTimer >= knifeCooldown)
            {
                knifeCooldownTimer = 0;
            }
        }*/
    }

    private void reloadGun()
    {
        int bulletNeeded = 7 - bulletAmount;
        if (bulletMax <= 0)
        {

        }
        else if (bulletMax < bulletNeeded)
        {
            // reload with amount of bulletmax
            StartCoroutine(Reloading(bulletMax + bulletAmount, bulletMax));
        }
        else if (bulletMax >= bulletNeeded)
        {
            StartCoroutine(Reloading(7, bulletNeeded));
        }
        //StartCoroutine(ReloadTime());
    }

    public void setBullets(int bAmount, int bMax)
    {
        bulletAmount = bAmount;
        bulletMax = bMax;
    }

    public int getBulletAmount()
    {
        return bulletAmount;
    }
    public int getBulletMax()
    {
        return bulletMax;
    }

    IEnumerator Reloading(int bulletReloaded, int bulletSubtracted)
    {
        isReloading = true;
        bulletAmount = 0;
        yield return new WaitForSeconds(2.5f);
        bulletMax -= bulletSubtracted;
        bulletAmount = bulletReloaded;
        isReloading = false;
    }

    IEnumerator KnifeAttack()
    {
        isKnifing = true;
        yield return new WaitForSeconds(0.3f);
        isKnifing = false;
        knifeSwing.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var targ = collision.gameObject.GetComponent<EnemyController>();
        if (collision.CompareTag("Enemy") && isKnifing)
        {
            targ.killed();
        }
        if (collision.CompareTag("Bullet") && isKnifing)
        {
            Destroy(collision.gameObject);
        }
    }

    public float getShotTimer()
    {
        return shotTimer;
    }

    public float getShotRate()
    {
        return shotRate;
    }

    public float getKnifeRate()
    {
        return knifeCooldown;
    }

    public float getKnifeCTimer()
    {
        return knifeCooldownTimer;
    }

    public bool getShooting()
    {
        return isShooting;
    }
}
