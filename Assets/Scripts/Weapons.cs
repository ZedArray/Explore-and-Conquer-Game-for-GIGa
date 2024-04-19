using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shotPoint;
    [SerializeField] Collider2D knifeHitBox;

    bool isKnifing;
    bool isShooting;
    float knifeTimer;
    float knifeCooldownTimer;
    float knifeCooldown = 1;
    float shotTimer;
    float shotRate;

    public float bulletSpeed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        isKnifing = false;
        knifeTimer = 0;
        knifeCooldownTimer = 0;
        shotTimer = 0.5f;
        shotRate = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;
        knifeCooldownTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shotTimer >= shotRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shotPoint.right * bulletSpeed, ForceMode2D.Impulse);
            shotTimer = 0f;
        }

        if (shotTimer == 0f)
        {
            isShooting = true;
        }
        else if (shotTimer >= 0.1f)
        {
            isShooting = false;
        }

        if (Input.GetMouseButton(1) && knifeCooldownTimer >= knifeCooldown)
        {
            if (knifeTimer > 0.2f)
            {
                isKnifing = false;
            }
            else
            {
                isKnifing = true;
                knifeTimer += Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isKnifing = false;
            knifeTimer = 0;
            if(knifeCooldownTimer >= knifeCooldown)
            {
                knifeCooldownTimer = 0;
            }
        }
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

    public bool getShooting()
    {
        return isShooting;
    }
}
