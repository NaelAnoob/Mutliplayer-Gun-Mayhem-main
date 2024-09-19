using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KillBoxHandler : NetworkBehaviour
{
    public LayerMask targetLayers;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) { return; }
        if ((targetLayers & (1 << collision.gameObject.layer)) != 0)
        {
            //May have to make a server RPC?
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.life.Value -= 1;
            if (playerHealth.life.Value > 0)
            {
                playerHealth.health.Value = playerHealth.maxHealth;
                playerHealth.transform.position = new Vector3(0, 10, 0);
            }
            else
            {
                collision.GetComponent<NetworkObject>().Despawn();
                collision.gameObject.SetActive(false);
            }
        }

    }
}
