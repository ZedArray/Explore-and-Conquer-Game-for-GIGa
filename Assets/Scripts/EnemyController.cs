using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] FieldOfView fovPrefab;
    FieldOfView fieldOfView;

    private float timer;
    public Transform[] points;
    int current;
    public float speed;
    int targetDir;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        transform.right = points[current + 1].position - points[current].position;

        fieldOfView = Instantiate(fovPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (current + 1 == points.Length)
        {
            targetDir = 0;
        }
        else
        {
            targetDir = current + 1;
        }

        timer += Time.deltaTime;
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(transform.right);
        if (transform.position != points[current].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
            timer = 0;
        }
        else
        {
            if (timer > 4)
            {
                float angle = Mathf.Atan2(points[targetDir].position.y - points[current].position.y, points[targetDir].position.x - points[current].position.x) * Mathf.Rad2Deg;
                var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
            }
            if (timer > 10)
            {
                current = (current + 1) % points.Length;
            }
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
            Destroy(gameObject);
            Destroy(fieldOfView.gameObject);
        }
    }

    public void killed()
    {
        Destroy(gameObject);
        Destroy(fieldOfView.gameObject);
    }

}
