using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using TMPro;
public class LobbyListUIElement : MonoBehaviour
{
    [HideInInspector]
    public Lobby lobby;
    public TMP_Text lobbyName;
    public TMP_Text players;
    public TMP_Text access;

    public void DisplayLobbyUI()
    {
        lobbyName.text = lobby.Name;
        players.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        access.text = lobby.IsPrivate ? "private" : "public";
    }
}