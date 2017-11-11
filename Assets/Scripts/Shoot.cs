using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform Container;
    public float Forcefullness = 10;

    public float PullbackForce
    {
        get
        {
            return _pullbackStart <= 0.1 ? 0 : (Time.time - _pullbackStart) * Forcefullness;
        }
    }

    private float _pullbackStart = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartPullback();
        }
        else if (Input.GetButtonUp("Fire1") && Math.Abs(_pullbackStart) > 0.1)
        {
            Fire();
        }
    }

    private void StartPullback()
    {
        _pullbackStart = Time.time;
    }

    private void Fire()
    {
        var bullet = Instantiate(
            BulletPrefab,
            transform.position,
            transform.rotation,
            Container
        );

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * PullbackForce;
        _pullbackStart = 0;
    }
}