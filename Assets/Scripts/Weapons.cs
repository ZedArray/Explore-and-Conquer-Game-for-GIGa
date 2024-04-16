using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shotPoint;
    [SerializeField] Collider2D knifeHitBox;

    bool isKnifing;

    public float bulletSpeed = 40f;
    // Start is called before the first frame update
    void Start()
    {
        isKnifing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shotPoint.right * bulletSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetMouseButtonDown(1))
        {
            isKnifing = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isKnifing = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var targ = collision.gameObject.GetComponent<EnemyController>();
        if (collision.CompareTag("Enemy") && isKnifing)
        {
            targ.killed();
        }
    }
}
