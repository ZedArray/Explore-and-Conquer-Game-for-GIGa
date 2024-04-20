using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Animator explosionAnim;
    [SerializeField] AudioSource explosionSFX;
    // Start is called before the first frame update
    void Awake()
    {
        explosionAnim.SetTrigger("Blow");
        explosionSFX.Play();
        StartCoroutine(DeleteThis());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (explosionAnim.GetCurrentAnimatorStateInfo(0).IsName("Default"))
        {
            explosionAnim.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }

    IEnumerator DeleteThis()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
