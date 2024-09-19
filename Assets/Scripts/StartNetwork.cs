using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartNetwork : MonoBehaviour
{
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        this.gameObject.SetActive(false);
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        this.gameObject.SetActive(false);
    }
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        this.gameObject.SetActive(false);
    }
}
