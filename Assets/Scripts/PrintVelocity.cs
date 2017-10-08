using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintVelocity : MonoBehaviour {

    public double HandRotationOrigin = 90;
	public double HandRotationThreshhold = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var body = this.GetComponent<Rigidbody>();
		Debug.Log("Hi! " + body.transform.eulerAngles.z);
        //Debug.Log(string.Format("Velocity: {0}", body.velocity.normalized));
        if(body.transform.eulerAngles.z < HandRotationOrigin + HandRotationThreshhold
           && body.transform.eulerAngles.z > HandRotationOrigin - HandRotationThreshhold){
            Debug.Log("Counts!");
        }
	}
}
