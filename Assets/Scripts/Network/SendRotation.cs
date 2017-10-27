using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SendRotation : NetworkBehaviour
{
    [SyncVar]
    public Vector3 syncedRotation = new Vector3();
    public bool sendToServer = true;

    public void FixedUpdate()
    {
        Debug.Log("Rotation: " + syncedRotation);
        if (isServer && sendToServer)
        {
            syncedRotation = transform.rotation.eulerAngles;
        }
        else
        {
            transform.rotation = Quaternion.Euler(syncedRotation);
            // use Quaternion.Lerp(); for smooth transition
        }
    }
}