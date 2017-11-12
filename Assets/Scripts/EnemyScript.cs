using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Transform[] DeactivateOnHit;
    public GameObject Explosion;

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
            StartCoroutine(LateCall(collision));
        }
    }

    IEnumerator LateCall(Collision collision)
    {
        foreach (var deactivate in DeactivateOnHit)
        {
            deactivate.gameObject.SetActive(false);
        }

        var newEx = Instantiate(
            Explosion,
            collision.transform.position,
            collision.transform.rotation
        );
        newEx.SetActive(true);
        Destroy(newEx, 10f);

        GetComponent<Orbit>().enabled = false;
        yield return new WaitForSeconds(Random.Range(8,12));

        GetComponent<Orbit>().enabled = true;
        foreach (var deactivate in DeactivateOnHit)
        {
            deactivate.gameObject.SetActive(true);
        }
    }
}
