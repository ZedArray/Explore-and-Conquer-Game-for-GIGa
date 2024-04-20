using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] PlayerController player;
    [SerializeField] Transform shotPoint;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] LayerMask playerMask;
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip gunShotSFX;
    
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] FieldOfView fovPrefab;
    FieldOfView fieldOfView;
    [SerializeField] AudioSource deadSFXprefab;

    public enum State
    {
        Idle,
        Seeing,
        Alert,
        Attacking,
        CoolingDown
    }

    private float timer;
    public Transform[] points;
    int current;
    int targetDir;
    float distanceFromPlayer;
    float bulletSpeed = 15f;
    float guntimer;
    private State state;
    float angle;
    Quaternion targetRotation;
    int bulletCounter;
    float alertTimer;
    float alertWhen = 5;
    float alertFOV = 150f;
    float originalFOV;

    bool wasAggroed;
    bool playerShot;

    [SerializeField] float speed;
    [SerializeField] int waitUntilTurn;
    [SerializeField] int waitUntilMove;
    [SerializeField] float FOV;
    [SerializeField] float viewDistance;

    private void Awake()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
        player = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[0].position;
        current = 0;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(points[1].position.y - points[0].position.y, points[1].position.x - points[0].position.x) * Mathf.Rad2Deg));

        fieldOfView = Instantiate(fovPrefab);

        distanceFromPlayer = 10f;
        guntimer = 0;
        state = State.Idle;
        fieldOfView.SetViewDistance(viewDistance);
        bulletCounter = 0;
        alertTimer = 0;
        wasAggroed = false;
        originalFOV = FOV;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.getIfPaused())
        {
            return;
        }
        if (gm.getIsDead())
        {
            state = State.Idle;
        }
        else
        {
            detectCheck();
        }
        fieldOfView.SetFOV(FOV);
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(transform.right);
        guntimer += Time.deltaTime;
        
        if (state != State.Alert || state != State.CoolingDown)
        {
            FOV = originalFOV;
        }

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
                    alertTimer += Time.deltaTime * 1.5f;
                    if (wasAggroed)
                    {
                        alertTimer += Time.deltaTime * 1.5f;
                    }
                }
                else if (!player.getCrouched())
                {
                    alertTimer += Time.deltaTime * 3;
                    if (wasAggroed)
                    {
                        alertTimer += Time.deltaTime * 3;
                    }
                }
                break;

            case State.Attacking:
                //spriteRenderer.color = new Color(255, 0, 0);
                aimAtPlayer();
                wasAggroed = true;
                
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
                    if (guntimer > 2f)
                    {
                        guntimer = 0;
                        bulletCounter = 0;
                    }
                }
                alertTimer = alertWhen + 3;
                break;

            case State.Alert:
                alertTimer -= Time.deltaTime;
                wasAggroed = true;
                FOV = alertFOV;
                break;

            case State.CoolingDown:
                FOV = alertFOV;
                alertTimer -= Time.deltaTime / 2;
                break;
        }
        
        /*float angle = Mathf.Atan2(player.position.y - transform.position.y, player.position.x - transform.position.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;*/
    }

    public bool getAggro()
    {
        return wasAggroed;
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
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        playerShot = player.getShooting();
        if (playerShot && distanceFromPlayer < 7f)
        {
            alertTimer = alertWhen + 3f;
            aimAtPlayer();
        }
        else if (distanceFromPlayer < viewDistance)
        {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.right, dirToPlayer) < FOV / 2f)
            {
                RaycastHit2D _hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, 0), dirToPlayer, viewDistance, playerMask);
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), dirToPlayer);
                //Debug.Log(_hit.transform.gameObject.name);
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
                    if (alertTimer >= alertWhen)
                    {
                        state = State.Alert;
                    }
                    else if (alertTimer < alertWhen && alertTimer > 0 && wasAggroed)
                    {
                        state = State.CoolingDown;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }
            }
            else if (!player.getCrouched() && distanceFromPlayer < 3f)
            {
                aimAtPlayer();
                alertTimer = alertWhen / 1.5f;
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
        audioPlayer.clip = gunShotSFX;
        audioPlayer.Play();
    }

    public void killed()
    {
        Instantiate(deadSFXprefab);
        Destroy(fieldOfView.gameObject);
        Destroy(gameObject);
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
