using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelWiseData{

 public int maxInventoryItems;

}

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

    [System.Serializable]
    public class ItemMax
    {
        public string itemName;
        public int maxCapacity;
    }

    public List<LevelWiseData> levelWiseDatas;

    public List<ItemMax> maxItems;


    private void Awake()
    {
        Instance = this;
    
    }

  
    public List<CollectableItem> inventoryItems = new List<CollectableItem>();

      public int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel",0);
    }

    public void AddItemsInInventory(string nameOfInventory, int quantity)
    {
        var existingInventoryItem = inventoryItems.Find(item => item.itemName == nameOfInventory);

        if (existingInventoryItem != null)
        {
            existingInventoryItem.itemQuantity += quantity;
        }
        else if (inventoryItems.Count < levelWiseDatas[currentLevel].maxInventoryItems)
        {
            inventoryItems.Add(new CollectableItem(nameOfInventory, quantity));
        }
        else
        {
            Debug.Log("Inventory is full");
            
        }
    }


    public int GetMaxCapcityFor(string itemName)
    {
        var itemMax = maxItems.Find(f => f.itemName == itemName);
        if (itemMax != null)
        {
            return itemMax.maxCapacity;
        }
        else
        {
            return 0;
        }
    }
}