using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer sRend;
    [SerializeField] Weapons weapons;
    /*[SerializeField] FieldOfView fieldOfView;*/

    [SerializeField] float moveSpeed;
    [SerializeField] float OriginalMoveSpeed;
    [SerializeField] float CrouchSpeed;
    Vector2 movement;
    Vector2 mousePosition;

    bool isCrouching;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        /*sRend.color = new Color(1f , 1f, 1f, 0.5f);*/
        CrouchSpeed = moveSpeed / 2;
        OriginalMoveSpeed = moveSpeed;
        isCrouching = false;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        isShooting = weapons.getShooting();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            sRend.color = new Color(1f, 1f, 1f, 0.5f);
            moveSpeed = CrouchSpeed;
            isCrouching = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            sRend.color = new Color(1f, 1f, 1f, 1f);
            moveSpeed = OriginalMoveSpeed;
            isCrouching = false;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        /*fieldOfView.SetAimDirection(lookDir);
        fieldOfView.SetOrigin(transform.position);*/

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }*/
    }

    public bool getShooting()
    {
        return isShooting;
    }

    public bool getCrouched()
    {
        return isCrouching;
    }
}
