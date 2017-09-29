using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour {
	public Vector3 speedVector = new Vector3 (0,0,0.1f);

	// Use this for initialization
	void Start () {
		Debug.Log ("Startet hier");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Update...");

		this.transform.position = this.transform.position + speedVector;

		Debug.Log ("Update done");
	}
}
