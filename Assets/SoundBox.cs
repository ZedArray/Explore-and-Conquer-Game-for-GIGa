using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    bool isEmittingGunshot;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isEmittingGunshot = true;
        }
        else
        {
            isEmittingGunshot = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && isEmittingGunshot)
        {
            Debug.Log("balls");
        }
    }
}
