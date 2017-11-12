using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShootForceScript : MonoBehaviour {
    public Shoot ShootObject;
	public int RateOverTime = 20;

    public float DefaultSpeedModifier = 180;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;
		var em = ps.emission;

        if (ShootObject.PullbackForce >= 0.1){
            em.rateOverTimeMultiplier = RateOverTime * ShootObject.PullbackForce;
            main.startSpeedMultiplier = ShootObject.PullbackForce * DefaultSpeedModifier;
        } else {
            em.rateOverTimeMultiplier = 0;
        }
	}
}
