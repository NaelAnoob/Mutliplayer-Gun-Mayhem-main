using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
public class DisplayPlayer : MonoBehaviour
{
    [HideInInspector]
    public NetworkObject playerToDisplay;
    public TMP_Text healthElement;
    public TMP_Text livesElement;

    private Health healthScript;

    private void Start()
    {
        healthScript = playerToDisplay.GetComponent<Health>();
    }
    private void Update()
    {
        float percentageOfHealth = ((float)healthScript.health.Value / (float)healthScript.maxHealth) * 100f;
        healthElement.text = percentageOfHealth.ToString() + " %";
        livesElement.text = healthScript.life.Value.ToString() + " lives";
    }


}
