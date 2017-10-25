using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SynchronizeRotation : NetworkBehaviour {

    [SyncVar]
    public Vector3 syncedRotation = new Vector3();

    public bool serverIsReceiver = true;

    public void FixedUpdate()
    {
        bool isReceiver = isServer && serverIsReceiver || !isServer && !serverIsReceiver;
        if (isReceiver)
        {
			transform.rotation = Quaternion.Euler(syncedRotation);
            Debug.Log("SetRotation: " + syncedRotation);
            Debug.Log(transform.rotation.Equals(Quaternion.Euler(syncedRotation)));
            // use Vector3.Lerp(); for smooth transition
        }
        else // isSender
        {
            if (isServer)
            {
                syncedRotation = transform.rotation.eulerAngles;
            }
            else
            { // client sets syncvar over commands
                CmdSetRotation(transform.rotation.eulerAngles);
            }
            Debug.Log("SendRotation: " + syncedRotation);
        }
    }

    [Command]
    void CmdSetRotation(Vector3 newRot)
    {
        syncedRotation = newRot;
    }
}
