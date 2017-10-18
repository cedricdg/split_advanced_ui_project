using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPhone : MonoBehaviour {
    public Transform yRotationTarget;

	// Use this for initialization
	void Start () {
        Debug.Log("Gyro enabled: " + Input.gyro.enabled);
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion deviceRotation = Input.gyro.attitude;
        Debug.Log(deviceRotation);
        Vector3 newEulerRotation = deviceRotation.eulerAngles;
        Vector3 oldEulerRotation = transform.rotation.eulerAngles;
        transform.rotation.eulerAngles.Set(newEulerRotation.x, oldEulerRotation.y, oldEulerRotation.z);
        Vector3 oldEulerRotationY = yRotationTarget.transform.rotation.eulerAngles;
        yRotationTarget.transform.rotation.eulerAngles.Set(oldEulerRotationY.x, newEulerRotation.y, oldEulerRotationY.z);
	}
}
