using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentItemObject : ItemObject
{
    public float atkBonus;
    public float defenceBonus;

    public GameObject equipementObject;
    public AttributeType equipementBodyPart;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
public enum AttributeType {
    Head,
    Body,
    LeftHand,
    RightHand,
    Necklace,
    Hair,
    Eyes
}