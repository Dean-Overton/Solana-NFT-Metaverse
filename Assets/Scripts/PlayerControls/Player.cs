using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float itemReachableDistance;
    [SerializeField] private Transform handPos;
    public LayerMask itemDropMask;

    public InventoryObject playerInventory;

    private List<Collider2D> pickupableDrops = new List<Collider2D>();

    [System.Serializable]
    public struct EquippedObject {
        public AttributeType type;
        public EquipmentItemObject equipementItem;
    }
    public EquippedObject[] defaultEquipped;
    public Dictionary<AttributeType, EquipmentItemObject> equipped = new Dictionary<AttributeType, EquipmentItemObject>();

    private void Start() {
        foreach(EquippedObject equipement in defaultEquipped) {
            equipped.Add(equipement.type, equipement.equipementItem);
        }
    }

    private void Update()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(handPos.position, itemReachableDistance, itemDropMask);
        List<Collider2D> currentPickUpDrops = cols.Cast<Collider2D>().ToList();
        foreach (Collider2D col in pickupableDrops) {
            if (!currentPickUpDrops.Contains(col)) {
                col.gameObject.GetComponent<Tag>().HideTag();
            } else {
                col.gameObject.GetComponent<ItemDrop>().ShowPickUpTag(); 
            }
        }
        pickupableDrops = currentPickUpDrops;

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Collider2D col in pickupableDrops) {
                //PICKUP
                ItemObject item = col.gameObject.GetComponent<ItemDrop>().item;
                if (item)
                    playerInventory.AddItem(item, 1);

                if (item.type == ItemType.Equipment){
                    EquipItem(item as EquipmentItemObject);
                }

                //GameEvents.current.PickUpItem(item);

                //**PICKUP ANIMATION**

                pickupableDrops.Remove(col);
                col.gameObject.GetComponent<ItemDrop>().PickUp();
            }
        }
    }
    public void EquipItem (EquipmentItemObject item) {
        GameObject itemObj = GameObject.Find(item.equipementObject.name);
        if (itemObj.activeSelf) {
            Debug.Log("Item already equipped!");
            return;
        }
        switch (item.equipementBodyPart)  
            {  
            case AttributeType.Body:  
                //Statement  
                break;  
            case AttributeType.LeftHand:
                EquipLeftHand(item, itemObj);
                break;  
            case AttributeType.RightHand:  
                //Statement  
                break;  
            default:  
                //Does not have an equippable body type
                break;  
            }  
    }
    public void EquipLeftHand (EquipmentItemObject itemToEquip, GameObject obj) {
        GameObject equipementItem = obj;
        EquipmentItemObject equippedHand;
        equipped.TryGetValue(AttributeType.LeftHand, out equippedHand);
        GameObject currentHand = GameObject.Find(equippedHand.name);
        currentHand.SetActive(false);
        if (!equipementItem)
            equipementItem = Instantiate(itemToEquip.equipementObject, currentHand.transform.parent);
        else
            equipementItem.SetActive(true);
        
        GetComponent<SortingScript>().sRS.Insert(0, equipementItem.GetComponent<SpriteRenderer>());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(handPos.position, itemReachableDistance);
    }
    private void OnApplicationQuit()
    {
        playerInventory.Container.Clear();
    }
}
