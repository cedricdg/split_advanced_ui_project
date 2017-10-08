using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnDestroy : MonoBehaviour {

    public GameObject[] ObjectsToActivate;

    void OnDestroy(){
        foreach (var go in ObjectsToActivate)
        {
            go.SetActive(true);
        }
    }
}
