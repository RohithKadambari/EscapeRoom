using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [System.Serializable]
    public class CollectableItem
    {
        public string itemName;
        public int itemQuantity;

        public CollectableItem(string itemName, int itemQuantity)
        {
            this.itemName = itemName;
            this.itemQuantity = itemQuantity;
        }
        
    }

    public bool canCollect;
    private void Awake()
    {
        Instance = this;
        canCollect= true;
    }
    public List<CollectableItem> inventoryItems = new List<CollectableItem>();
    [SerializeField] int maxInventoryItems;
    public void AddItemsInInventory(string nameOfInventory,int quantity)
    {
        var existingInventoryItem = inventoryItems.Find(item => item.itemName == nameOfInventory);

        if (existingInventoryItem != null)
        {
            existingInventoryItem.itemQuantity += quantity;
        }
        else if(inventoryItems.Count <= maxInventoryItems && canCollect)
        {
            inventoryItems.Add(new CollectableItem(nameOfInventory,quantity));
        }
        else
        {
            Debug.Log("Inventory is full");
            canCollect= false;
        }
    }
}
