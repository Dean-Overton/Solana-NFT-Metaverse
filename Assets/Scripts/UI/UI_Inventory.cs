using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Inventory : MonoBehaviour
{
    public InventoryObject inventory;

    public GameObject inventorySlotPrefab;

    private Transform itemSlotContainer;
    [SerializeField] private GameObject inventoryItemTemplate;
    private Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
    }
    private void Start()
    {
        InitialiseInventoryUI();
    }
    public void SetInventory (InventoryObject inventory)
    {
        this.inventory = inventory;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenInventory();
    }
    public void OpenInventory ()
    {
        gameObject.SetActive(true);
    }
    public void InitialiseInventoryUI()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            CreateItemSlot(i);
            if (itemSlotContainer)
                itemSlotContainer.gameObject.SetActive(true);
        }
    }
    public void CreateItemSlot (int i)
    {
        if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            return;

        RectTransform itemSlotTransform = Instantiate(inventorySlotPrefab, itemSlotContainer).GetComponent<RectTransform>();
        Text itemName = itemSlotTransform.GetChild(3).GetComponent<Text>();
        Text itemAmount = itemSlotTransform.GetChild(2).GetComponent<Text>();
        Image itemImageIcon = itemSlotTransform.GetChild(1).GetComponent<Image>();

        itemAmount.text = inventory.Container[i].amount.ToString();
        itemName.text = inventory.Container[i].item.name;
        string path = string.Concat("Items/ItemIconSprites/", inventory.Container[i].item.name);
        Sprite icon = (Sprite)Resources.Load(path, typeof(Sprite));
        if (icon)
            itemImageIcon.sprite = icon;

        itemsDisplayed.Add(inventory.Container[i], itemSlotTransform.gameObject);
    }
    private void OnEnable()
    {
        UpdateInventoryUI();
    }
    public void UpdateInventoryUI ()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].transform.GetChild(2).GetComponent<Text>().text = inventory.Container[i].amount.ToString();
            } else
            {
                CreateItemSlot(i);
            }
        }
    }
}
