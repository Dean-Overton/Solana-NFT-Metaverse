using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialisedItemPickup : MonoBehaviour
{
    public ItemObject item;

    public GameObject enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onItemPickup += PickUp;
    }
    public void PickUp (ItemObject item) {
        if (item == this.item) {
            enemySpawner.SetActive(true);
        }
    }
}
