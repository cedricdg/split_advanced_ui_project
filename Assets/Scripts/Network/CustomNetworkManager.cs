using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {


    public GameObject enableOnServerConnect;

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        enableOnServerConnect.SetActive(true);
    }
}
