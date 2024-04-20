using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameManager gm;
    // Start is called before the first frame update

    private void Awake()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!gm.getIsDead())
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10);
        }
    }
}
