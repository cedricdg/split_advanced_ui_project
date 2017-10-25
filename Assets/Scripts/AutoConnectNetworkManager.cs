using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class AutoConnectNetworkManager : MonoBehaviour {

    public RuntimePlatform[] AutoStartOnPlatform = { RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
    public bool StartAsHost = false;

    bool isEditor = false;
    NetworkManager manager;
    // Use this for initialization
    void Awake()
    {
        manager = GetComponent<NetworkManager>();
#if UNITY_EDITOR
        isEditor = true;
#endif
    }

	// Use this for initialization
	void Start () {
        if(!isEditor && AutoStartOnPlatform.Contains(Application.platform)){
            if(StartAsHost){
				manager.StartHost();
            } else {
                manager.StartClient();                
            }
            Debug.Log("Started Host automatically");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
