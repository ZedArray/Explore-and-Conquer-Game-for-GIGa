using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform shotPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] FieldOfView fovPrefab;
    FieldOfView fieldOfView;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] LayerMask playerMask;

    public enum State
    {
        Idle,
        Seeing,
        Alert,
        Attacking,
        CoolingDown
        //Idle = Basic State
        //Alert = Shoot on sight
        //Attacking = Shooting
    }

    private float timer;
    public Transform[] points;
    int current;
    int targetDir;
    float distanceFromPlayer;
    float bulletSpeed = 10f;
    float guntimer;
    private State state;
    float angle;
    Quaternion targetRotation;
    int bulletCounter;
    float alertTimer;
    float alertWhen = 5;

    bool wasAggroed;

    [SerializeField] float speed;
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
        bulletCounter = 0;
        alertTimer = 0;
        wasAggroed = false;
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
                //spriteRenderer.color = new Color(255, 255, 255);
                wasAggroed = false;
                if (current + 1 == points.Length)
                {
                    targetDir = 0;
                }
                else
                {
                    targetDir = current + 1;
                }

                if (alertTimer > 0)
                {
                    alertTimer -= Time.deltaTime;
                }
                else if (alertTimer < 0)
                {
                    alertTimer = 0;
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

            case State.Seeing:
                //spriteRenderer.color = new Color(255, 255, 0);
                aimAtPlayer();
                if (player.getCrouched())
                {
                    alertTimer += Time.deltaTime;
                }
                else if (!player.getCrouched())
                {
                    alertTimer += Time.deltaTime * 3;
                }
                break;

            case State.Attacking:
                //spriteRenderer.color = new Color(255, 0, 0);
                aimAtPlayer();
                wasAggroed = true;
                guntimer += Time.deltaTime;
                
                if(bulletCounter < 3)
                {
                    if (guntimer > 0.2f)
                    {
                        shoot();
                        guntimer = 0;
                        bulletCounter++;
                    }
                }
                else if (bulletCounter == 3){
                    if (guntimer > 3f)
                    {
                        guntimer = 0;
                        bulletCounter = 0;
                    }
                }
                alertTimer = alertWhen + 3;
                break;

            case State.Alert:
                alertTimer -= Time.deltaTime;
                break;

            case State.CoolingDown:
                alertTimer -= Time.deltaTime / 2;
                break;
        }
        
        /*float angle = Mathf.Atan2(player.position.y - transform.position.y, player.position.x - transform.position.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;*/
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    public State getState()
    {
        return state;
    }

    private void aimAtPlayer()
    {
        angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
    }

    private void detectCheck()
    {
        if (distanceFromPlayer < viewDistance)
        {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.right, dirToPlayer) < FOV / 2f)
            {
                RaycastHit2D _hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, 0), dirToPlayer, viewDistance, playerMask);
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), dirToPlayer);
                Debug.Log(_hit.transform.gameObject.name);
                if(_hit.transform.gameObject.name == "Player")
                {
                    if (alertTimer > alertWhen)
                    {
                        state = State.Attacking;
                    }
                    else
                    {
                        state = State.Seeing;
                    }
                }
                else
                {
                    if (wasAggroed)
                    {
                        state = State.CoolingDown;
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
            if (alertTimer >= alertWhen)
            {
                state = State.Alert;
            }
            else if (alertTimer < alertWhen && alertTimer > 0)
            {
                if(wasAggroed)
                {
                    state = State.CoolingDown;
                }
                else
                {
                    state = State.Idle;
                }
            }
            else if (alertTimer <= 0)
            {
                state = State.Idle;
            }
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

    public float getAlertTimer()
    {
        return alertTimer;
    }

    public float getAlertWhen()
    {
        return alertWhen;
    }
}
