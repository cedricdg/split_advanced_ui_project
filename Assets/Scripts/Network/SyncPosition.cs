
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class SyncPosition : NetworkBehaviour
{
    [SyncVar]
    public Vector3 syncedPosition = new Vector3();

    public int NetworkChannel = 0;
    public float NetworkSendInterval = 0.1f;

    public bool ServerIsReceiver = true;

    public void Start()
    {
        syncedPosition = transform.position;
    }

    public void FixedUpdate()
    {
        bool isReceiver = isServer && ServerIsReceiver || !isServer && !ServerIsReceiver;
        if (isReceiver)
        {
            if (!transform.position.Equals(syncedPosition))
            {
                Debug.Log("SetPosition: " + syncedPosition);
                transform.position = syncedPosition;
                // use Vector3.Lerp(); for smooth transition
            }
        }
        else // isSender
        {
            if (isServer)
            {
                syncedPosition = transform.position;
            }
            else
            { // client sets syncvar over commands
                CmdSetPosition(transform.rotation.eulerAngles);
            }
            Debug.Log("SendPosition: " + syncedPosition);
        }
    }


    [Command]
    void CmdSetPosition(Vector3 newPos)
    {
        syncedPosition = newPos;
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
