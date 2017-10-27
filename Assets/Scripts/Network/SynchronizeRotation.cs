using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SynchronizeRotation : NetworkBehaviour {

    [SyncVar]
    public Vector3 syncedRotation = new Vector3();

    public int NetworkChannel = 0;
    public float NetworkSendInterval = 0.1f;

    public bool ServerIsReceiver = true;

    public void FixedUpdate()
    {
        bool isReceiver = isServer && ServerIsReceiver || !isServer && !ServerIsReceiver;
        if (isReceiver)
        {
            if(!transform.rotation.eulerAngles.Equals(syncedRotation)){
                Debug.Log("SetRotation: " + syncedRotation);
                transform.rotation = Quaternion.Euler(syncedRotation);
                // use Vector3.Lerp(); for smooth transition
            }
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

    public override int GetNetworkChannel()
    {
        return NetworkChannel;
    }

    public override float GetNetworkSendInterval()
    {
        return NetworkSendInterval;
    }
}
