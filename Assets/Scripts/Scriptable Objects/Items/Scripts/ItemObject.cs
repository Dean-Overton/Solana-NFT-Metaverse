using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public GameObject itemDropPrefab;
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