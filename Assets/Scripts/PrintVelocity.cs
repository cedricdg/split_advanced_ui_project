using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintVelocity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var body = this.GetComponent<Rigidbody>();
        //Debug.Log(string.Format("Velocity: {0}", body.velocity.normalized));
        Debug.Log(body.rotation);
	}
}
