using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float healthPoints = 100f;
    public float itemReachableDistance;
    [SerializeField] private Transform handPos;
    public LayerMask itemDropMask;

    public InventoryObject playerInventory;

    private List<Collider2D> pickupableDrops = new List<Collider2D>();

    public Dictionary<AttributeType, EquipmentItemObject> equipped = new Dictionary<AttributeType, EquipmentItemObject>();

    private void Start() {
        if(playerInventory.Container.Count > 0) {
            foreach(InventorySlot slot in playerInventory.Container) {
                if (slot.item.type == ItemType.Equipment) {
                    AddEquipement(slot.item as EquipmentItemObject);
                    //if 
                }
            }
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
    public void AddEquipement (EquipmentItemObject item) {
        string itemBodyTypeString = "";
        switch (item.equipementBodyPart)  
        {  
            case AttributeType.Body:
                itemBodyTypeString = "Body";
                break;  
            case AttributeType.LeftHand:
                itemBodyTypeString = "Left Hand";
                break;  
            case AttributeType.RightHand:  
                itemBodyTypeString = "Right Hand"; 
                break;
        }
        Instantiate(item.equipementObject, GameObject.Find(itemBodyTypeString).transform); 
    }
    public void EquipItem (EquipmentItemObject item) {
        GameObject itemObj = GameObject.Find(item.equipementObject.name);
        if (itemObj.activeSelf) {
            Debug.Log("Item already equipped!");
            return;
        }
        //if (item.)
        switch (item.equipementBodyPart)  
        {  
            case AttributeType.Body:  
                //Statement  
                break;  
            case AttributeType.LeftHand:
                EnableLeftHand(itemObj);
                break;  
            case AttributeType.RightHand:  
                //Statement  
                break;  
            default:  
                //Does not have an equippable body type
                break;  
        }  
    }
    public void EnableLeftHand (GameObject obj) {
        EquipmentItemObject currentHand;
        if (equipped.TryGetValue(AttributeType.LeftHand, out currentHand)) {
            GameObject.Find(currentHand.name).SetActive(false);
            obj.SetActive(true);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(handPos.position, itemReachableDistance);
    }
}
