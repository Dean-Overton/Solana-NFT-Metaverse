using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public GameObject itemPrefab;
    public ItemType type;
    public bool stackable;
    [TextArea(15, 20)]
    public string description;
}
public enum ItemType
{
   Consumable,
   Equipment,
   Default
}