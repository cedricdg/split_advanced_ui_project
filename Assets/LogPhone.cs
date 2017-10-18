using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPhone : MonoBehaviour {
    public Vector3 targetRotation;
    public Transform yRotationTarget;

	void Start () {
        Debug.Log("Gyro enabled: " + Input.gyro.enabled);
        Input.gyro.enabled = true;
	}
	
	void Update () {
        Quaternion deviceRotation = Input.gyro.attitude;
        Debug.Log(deviceRotation);
        targetRotation = deviceRotation.eulerAngles;
        Vector3 oldEulerRotation = transform.rotation.eulerAngles;
        transform.rotation.eulerAngles.Set(targetRotation.x, oldEulerRotation.y, oldEulerRotation.z);
        Vector3 oldEulerRotationY = yRotationTarget.transform.rotation.eulerAngles;
        yRotationTarget.transform.rotation.eulerAngles.Set(oldEulerRotationY.x, targetRotation.y, oldEulerRotationY.z);
	}
}
