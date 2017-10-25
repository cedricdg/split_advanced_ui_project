
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class SyncPosition : NetworkBehaviour
{

    [SyncVar]
    public Vector3 syncedPosition = new Vector3();

    public bool serverIsReceiver = true;

    public void FixedUpdate()
    {
        Debug.Log("Position: " + syncedPosition);
        bool isReceiver = isServer && serverIsReceiver || !isServer && !serverIsReceiver;
        if (isReceiver)
        {
            transform.position = syncedPosition;
            // use Vector3.Lerp(); for smooth transition
        }
        else // isSender
        { 
            if(isServer){
                syncedPosition = transform.position;
            } else { // client sets syncvar over commands
                CmdSetPosition(transform.position);
            }
        }
    }

    [Command]
    void CmdSetPosition(Vector3 newPos)
    {
        syncedPosition = newPos;
    }
}
