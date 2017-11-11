using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Transform[] DeactivateOnHit;

    // Use this for initialization
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log("was bullet");
            StartCoroutine(LateCall());
        }
    }

    IEnumerator LateCall()
    {
        foreach (var deactivate in DeactivateOnHit)
        {
            deactivate.gameObject.SetActive(false);
        }

        GetComponent<Orbit>().enabled = false;
        yield return new WaitForSeconds(Random.Range(8,12));

        GetComponent<Orbit>().enabled = true;
        foreach (var deactivate in DeactivateOnHit)
        {
            deactivate.gameObject.SetActive(true);
        }
    }
}
