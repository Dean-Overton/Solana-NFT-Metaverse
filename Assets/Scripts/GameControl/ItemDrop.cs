using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop: MonoBehaviour
{
    public ItemObject item;
    
    [Tooltip("Leaving this black will result in the item appearing to still not be picked up.")]
    public GameObject[] destroyOnPickup;

    [Header("Item Drop Settings")]
    [Tooltip("Can the item be picked up?")]
    public bool pickupable = false;
    [Tooltip("A duration of -1 is an infinite duration item drop. Used most commonly for quest objects.")]
    [SerializeField]
    private float duration = -1f;
    //public string stateOfAnimation;

    float timeLeft = 0;
    private void Update() {
        if(duration != -1f) {
            if (timeLeft == 0)
                timeLeft = duration;
            else if (duration > 0) 
                timeLeft -= Time.deltaTime;
            else
                DestroyObjects();
        }
    }
    public void PickUp() {
        pickupable = false;
        GameEvents.current.PickUpItem(item);
        DestroyObjects();
        GetComponent<Collider2D>().enabled = false;
    }
    public void ShowPickUpTag () {
        if (pickupable) {
            GetComponent<Tag>().ChangeTag("Pickup (P)");
            GetComponent<Tag>().ShowTag();
        }
    }
    public void DestroyObjects () {
        foreach (GameObject obj in destroyOnPickup) {
            Destroy(obj);
        }
    }
}
