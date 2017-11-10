using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform SpawnPoint;
    public Transform Container;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {
        var bullet = Instantiate(
            BulletPrefab,
            SpawnPoint.position,
            SpawnPoint.rotation,
            Container
        );
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
    }
}