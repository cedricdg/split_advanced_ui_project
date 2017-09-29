using System.Collections;
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
        if (other.CompareTag("Scoreball")) {
			Debug.Log("1");
            Destroy(other);
            if(InstantiateOnTriggerEnter != null){
                Debug.Log("2");
                var copy = Instantiate(InstantiateOnTriggerEnter);
                copy.gameObject.SetActive(true);
            }
        }
    }

}
