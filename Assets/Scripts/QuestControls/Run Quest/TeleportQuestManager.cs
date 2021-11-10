using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportQuestManager : MonoBehaviour
{
    [SerializeField] private EquipmentItemObject teleportOrb;
    private Player playerScript;
    private void Awake() {
        playerScript = FindObjectOfType<Player>();
        playerScript.ovverideEquipped = true;
    }
    private void Start() {
        playerScript.AddEquipement(teleportOrb);
        playerScript.EquipItem(teleportOrb);
    }
}
