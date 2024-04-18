using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertTimer : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] EnemyController enemyController;
    [SerializeField] Material radFillMat;
    float alertTimer;
    float alertWhen;
    public float fillAmount;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.material = new Material(radFillMat);
        spriteRenderer.sharedMaterial.SetFloat("_Angle", 90);
        alertWhen = enemyController.getAlertWhen();
    }

    // Update is called once per frame
    void Update()
    {
        alertTimer = enemyController.getAlertTimer();
        fillAmount = 360 - (alertTimer / alertWhen * 360);

        switch (enemyController.getState())
        {
            case EnemyController.State.Seeing:
                spriteRenderer.color = Color.white;
                break;

            case EnemyController.State.Attacking:
                spriteRenderer.color = Color.red;
                break;

            case EnemyController.State.Idle:
                spriteRenderer.color = Color.white;
                break;

            case EnemyController.State.Alert:
                spriteRenderer.color = new Color32(255, 115, 0, 255);
                break;

            case EnemyController.State.CoolingDown:
                spriteRenderer.color = Color.yellow;
                break;
        }

        transform.rotation = new Quaternion(0, 0, 0, 1);
        spriteRenderer.sharedMaterial.SetFloat("_Arc1", fillAmount);
    }
}
