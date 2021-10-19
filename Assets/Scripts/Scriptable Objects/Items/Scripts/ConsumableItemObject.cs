using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItemObject : ItemObject
{
    public ConsumableType typeOfConsumable;
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
public enum ConsumableType
{
    Health,
    Poison,
    Potion
}
[CreateAssetMenu(fileName = "New Health Object", menuName = "Inventory System/Items/Consumable/Health")]
public class HealthItemObject : ConsumableItemObject
{
    public int restoreHealthAmount;
    public void Awake()
    {
        typeOfConsumable = ConsumableType.Health;
    }
}
[CreateAssetMenu(fileName = "New Poison Object", menuName = "Inventory System/Items/Consumable/Poison")]
public class PoisonItemObject : ConsumableItemObject
{
    public int poisonDuration;

    public void Awake()
    {
        typeOfConsumable = ConsumableType.Poison;
    }
}
[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Consumable/Potion")]
public class PotionConsumableItemObject : ConsumableItemObject
{
    public int potionDuration;

    public void Awake()
    {
        typeOfConsumable = ConsumableType.Potion;
    }
}