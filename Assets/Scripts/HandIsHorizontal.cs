using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIsHorizontal : MonoBehaviour
{

    public float HandRotationOrigin = 90;
    public float HandRotationThreshhold = 20;

    public bool IsHorizontal;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsValidAngle())
        {
            IsHorizontal = true;
        } else {
            IsHorizontal = false;
        }
    }

    private bool IsValidAngle()
    {
        return transform.eulerAngles.z < HandRotationOrigin + HandRotationThreshhold
               && transform.eulerAngles.z > HandRotationOrigin - HandRotationThreshhold;
    }
}
