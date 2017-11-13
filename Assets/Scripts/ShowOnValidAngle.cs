using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnValidAngle : MonoBehaviour {

    public CalculateYVelocity velocityCalculator;
    public GameObject ToBeShown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        ToBeShown.SetActive(velocityCalculator.IsHandValidAngle);
	}
}
