using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
public class Health : NetworkBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public NetworkVariable<int> health = new NetworkVariable<int>(100);

    public NetworkVariable<int> life = new NetworkVariable<int>(10);
    [Header("UI")]
    public TextMeshProUGUI healthText;
    public bool isLocalPlayer;

    private void Awake()
    {
        health.Value = maxHealth;
    }
    public void TakeDamage(int _damage)
    {
        TakeHealthServerRpc(_damage);
        //healthText.text = health.ToString();
        print("took damage");
        if (health.Value <= 0)
        {
            // if (isLocalPlayer)
            // {
            //     RoomManager.instance.SpawnPlayer();
            //     RoomManager.instance.deaths++;
            //     RoomManager.instance.SetHashes();
            // }
            Destroy(this.gameObject);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void TakeHealthServerRpc(int damage)
    {
        if (IsServer)
        {
            health.Value -= damage;
        }
    }
}
