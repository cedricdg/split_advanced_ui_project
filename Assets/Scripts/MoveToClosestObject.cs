using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClosestObject : MonoBehaviour {

    public string TargetTag = "Scoreball";
    private GameObject[] targets;
    public GameObject target;
    public float rotationSpeed;

    // Use this for initialization
    void Start () {
        targets = GameObject.FindGameObjectsWithTag(TargetTag);
        target = FindClosestWithTag();
	}

    private void FixedUpdate()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
    }


    public GameObject FindClosestWithTag()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in targets)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
