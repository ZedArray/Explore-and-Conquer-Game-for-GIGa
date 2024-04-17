using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform shotPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] FieldOfView fovPrefab;
    FieldOfView fieldOfView;

    enum State
    {
        Idle,
        Alert,
        Attacking
        //Idle = Basic State
        //Alert = Shoot on sight
        //Attacking = Shooting
    }

    private float timer;
    public Transform[] points;
    int current;
    public float speed;
    int targetDir;
    float distanceFromPlayer;
    float bulletSpeed = 10f;
    float guntimer;
    private State state;
    float angle;
    Quaternion targetRotation;

    [SerializeField] LayerMask playerMask;
    [SerializeField] int waitUntilTurn;
    [SerializeField] int waitUntilMove;
    [SerializeField] float FOV;
    [SerializeField] float viewDistance;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        transform.right = points[current + 1].position - points[current].position;

        fieldOfView = Instantiate(fovPrefab);

        guntimer = 0;
        state = State.Idle;
        fieldOfView.SetFOV(FOV);
        fieldOfView.SetViewDistance(viewDistance);
    }

    // Update is called once per frame
    void Update()
    {
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(transform.right);
        detectCheck();

        switch (state)
        {
            case State.Idle:
                if (current + 1 == points.Length)
                {
                    targetDir = 0;
                }
                else
                {
                    targetDir = current + 1;
                }

                timer += Time.deltaTime;
                if (transform.position != points[current].position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
                    timer = 0;
                }
                else
                {
                    if (timer > waitUntilTurn)
                    {
                        angle = Mathf.Atan2(points[targetDir].position.y - points[current].position.y, points[targetDir].position.x - points[current].position.x) * Mathf.Rad2Deg;
                        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
                    }
                    if (timer > waitUntilMove)
                    {
                        current = (current + 1) % points.Length;
                    }
                }
                break;
            case State.Attacking:
                angle = Mathf.Atan2(playerPosition.position.y - transform.position.y, playerPosition.position.x - transform.position.x) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                transform.rotation = targetRotation;
                guntimer += Time.deltaTime;
                if (guntimer > 2)
                {
                    shoot();
                    guntimer = 0;
                }
                break;
        }
        
        /*float angle = Mathf.Atan2(playerPosition.position.y - transform.position.y, playerPosition.position.x - transform.position.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;*/
        distanceFromPlayer = Vector3.Distance(playerPosition.position, transform.position);
    }

    private void detectCheck()
    {
        if (distanceFromPlayer < viewDistance)
        {
            Vector3 dirToPlayer = (playerPosition.position - transform.position).normalized;
            if(Vector3.Angle(transform.right, dirToPlayer) < FOV / 2f)
            {
                RaycastHit2D _hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, 0), dirToPlayer, viewDistance, playerMask);
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), dirToPlayer);
                if (_hit.collider != null)
                {
                    Debug.Log(_hit.transform.gameObject.name);
                    if(_hit.transform.gameObject.name == "Player")
                    {
                        state = State.Attacking;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }
            }
        }
        else
        {
            state = State.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("aaaa");
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            killed();
        }
    }

    private void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shotPoint.right * bulletSpeed, ForceMode2D.Impulse);
    }

    public void killed()
    {
        Destroy(gameObject);
        Destroy(fieldOfView.gameObject);
    }

}
