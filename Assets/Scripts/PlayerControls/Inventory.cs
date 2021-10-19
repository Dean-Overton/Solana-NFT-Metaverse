using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Inventory
{
    private List<Item> itemList;
    
    public Inventory ()
    {
        itemList = new List<Item>();

        AddItem(new Item("Jewel"));
    }
    public void AddItem (Item item)
    {
        if (itemList.Contains(item))
        {
            itemList[itemList.IndexOf(item)].itemAmount += 1;
        } else {
            itemList.Add(item);
        }
    }
    //Try to avoid using this function as it uses a foreach statment
    public void AddItem(string itemType, int itemAmount)
    {
        foreach(Item item in itemList)
        {
            if (item.itemType == itemType)
            {
                itemList[itemList.IndexOf(item)].itemAmount += 1;
                return;
            }
        }
        Item newItem = new Item(itemType, itemAmount);
        itemList.Add(newItem);
    }

    public void ConsumeItem(Item item)
    {
        if (itemList.Contains(item)) {
            int itemNewAmount = itemList[itemList.IndexOf(item)].itemAmount -= 1;
            if (itemNewAmount <= 0)
                itemList.Remove(itemList[itemList.IndexOf(item)]);
            else
                itemList[itemList.IndexOf(item)].itemAmount = itemNewAmount;
        }
    }

    public List<Item> GetItemList ()
    {
        return itemList;
    }
}
[System.Serializable]
public class Item
{
    [Tooltip("This is also the item type and needs a sprite icon with this exact same name.")]
    public string itemType;
    [Min(0)] public int itemAmount;

    public Item (string itemType, int itemAmount)
    {
        this.itemType = itemType;
        this.itemAmount = itemAmount;
    }
    public Item(string itemType)
    {
        this.itemType = itemType;
        this.itemAmount = 1;
    }
}
