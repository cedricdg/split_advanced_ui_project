using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class StartAsHost : MonoBehaviour {

    public RuntimePlatform[] AutoStartOnPlatform = { RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
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
            manager.StartHost();
            Debug.Log("Started Host automatically");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
