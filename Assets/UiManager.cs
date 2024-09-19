using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UiManager : NetworkBehaviour
{
    public GameObject uiParent;
    public GameObject playerUiElement;
    private NetworkObject localNetworkObject;
    public override void OnNetworkSpawn()
    {
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
        if (IsServer) return;
        foreach (var playerObject in FindObjectsOfType<PlayerStateMachine>())
        {
            ulong clientId = playerObject.GetComponent<NetworkObject>().OwnerClientId;
            if (NetworkManager.LocalClientId == clientId)
            {
                localNetworkObject = playerObject.GetComponent<NetworkObject>();
            }
            AddPlayerToUI(playerObject.GetComponent<NetworkObject>());
            Debug.Log("Player found for client " + clientId);
            break;
        }
        Debug.Log("Player found for client ");

    }

    private void OnDisable()
    {
        // Unsubscribe from the client connection callback
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
    private void OnClientConnected(ulong clientId)
    {

        // Find the player object associated with this client
        if(!IsClient) { return; }
        
        foreach (var networkObject in FindObjectsOfType<NetworkObject>())
        {
            if (networkObject.OwnerClientId == clientId)
            {
                if(NetworkManager.LocalClientId == clientId)
                {
                    localNetworkObject = networkObject;
                }
                AddPlayerToUI(networkObject);
                Debug.Log("Player found for client " + clientId);
                break;
            }
        }
    }
    private void AddPlayerToUI(NetworkObject playerObject)
    {
        if (uiParent != null && playerUiElement != null)
        {
            // Instantiate a new UI element and set its parent
            GameObject playerUI = Instantiate(playerUiElement, uiParent.transform);

            // Ensure the UI element has a script that can handle displaying player data
            DisplayPlayer display = playerUI.GetComponent<DisplayPlayer>();

            if (display != null)
            {
                display.playerToDisplay = playerObject; // Pass the player object reference to the UI
            }
            else
            {
                Debug.LogWarning("DisplayPlayer component not found on the instantiated UI element.");
            }
        }
        else
        {
            Debug.LogError("uiParent or playerUiElement is not assigned in the UiManager.");
        }
    }
}
