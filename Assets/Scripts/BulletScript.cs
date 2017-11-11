using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public GameObject Explosion;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {

            Debug.Log("Enter " + collision.collider.name);
            Destroy(gameObject, 0f);


            var newEx = Instantiate(
                Explosion,
                transform.position,
                transform.rotation
            );
            newEx.SetActive(true);
            Destroy(newEx, 10f);
        }
    }
}
