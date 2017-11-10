using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform SpawnPoint;
    public Transform Container;
    public float Forcefullness = 10;

    private float _pullbackStart = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartPullback();
        } else if (Input.GetButtonUp("Fire1") && Math.Abs(_pullbackStart) > 0.1)
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
            SpawnPoint.position,
            SpawnPoint.rotation,
            Container
        );
        var pullbackTime = Time.time - _pullbackStart;
        var force = pullbackTime * Forcefullness;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * force;
    }
}