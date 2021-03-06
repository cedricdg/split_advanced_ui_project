﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnTouch : MonoBehaviour {

    public string DestroyTag;
    public Transform InstantiateOnTriggerEnter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DestroyTag)) {
            Destroy(other);
            if(InstantiateOnTriggerEnter != null){
                var copy = Instantiate(InstantiateOnTriggerEnter);
                copy.transform.position = other.transform.position;
                copy.gameObject.SetActive(true);
            }
        }
    }

}
