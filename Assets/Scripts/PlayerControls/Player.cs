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

    private void Update()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, itemReachableDistance, itemDropMask);
        List<Collider2D> currentPickUpDrops = cols.Cast<Collider2D>().ToList();
        foreach (Collider2D col in pickupableDrops) {
            if (!currentPickUpDrops.Contains(col))
                if (col != null)
                    col.gameObject.GetComponent<ItemPickupTrigger>().ItemPickTip(false);
        }
        pickupableDrops = currentPickUpDrops;
        foreach (Collider2D col in pickupableDrops) {
            col.gameObject.GetComponent<ItemPickupTrigger>().ItemPickTip(true); }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) //Did we hit something?
            {
                if (hit.collider.tag == "Item Drop") //Is it an item we can pickup?
                {
                    if (Vector2.Distance(hit.transform.position, transform.position) <= itemReachableDistance) //Can we reach it?
                    {
                        //PICKUP
                        ItemObject item = hit.transform.gameObject.GetComponent<ItemPickupTrigger>().item;
                        if (item)
                            playerInventory.AddItem(item, 1);

                        GameEvents.current.PickUpItem(item);

                        //**PICKUP ANIMATION**

                        pickupableDrops.Remove(hit.collider);
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
#else
            if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null) //Did we hit something?
                {
                    if (hit.collider.tag == "Item Drop") //Is it an item we can pickup?
                    {
                        if (Vector2.Distance(hit.transform.position, transform.position) <= itemReachableDistance) //Can we reach it?
                        {
                            //PICKUP
                        ItemObject item = hit.transform.gameObject.GetComponent<ItemPickupTrigger>().item;
                        if (item)
                            playerInventory.AddItem(item, 1);

                        GameEvents.current.PickUpItem(item);

                        //**PICKUP ANIMATION**

                        pickupableDrops.Remove(hit.collider);
                        Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }
#endif
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
