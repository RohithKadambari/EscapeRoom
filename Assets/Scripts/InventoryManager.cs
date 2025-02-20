using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public bool canCollect;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canCollect = true;
        
    }

    [System.Serializable]
    public class InventorySystem
    {
        public string itemName;
        
        public int itemQuantity;

       public InventorySystem(string itemName, int itemQuantity)
        {
            this.itemName = itemName;
            this.itemQuantity = itemQuantity;
        }
        
    }

    public List<InventorySystem> inventory = new List<InventorySystem>();
    [SerializeField] private int maxInventoryCapacity;

    public void AddItemsToInventory(string inventoryItemName, int inventoryItemQuantity)
    {
        var existingItem = inventory.Find(item => item.itemName == inventoryItemName);

        if (existingItem != null && canCollect == true)
        {
            existingItem.itemQuantity = inventoryItemQuantity + existingItem.itemQuantity;
        }
        
        if (inventory.Count < maxInventoryCapacity && canCollect == true)
        { 
            inventory.Add(new InventorySystem(inventoryItemName,inventoryItemQuantity));
        }
        else
        { 
            canCollect = false;
            Debug.Log("Inventory is full");

        }
    }
    
}
