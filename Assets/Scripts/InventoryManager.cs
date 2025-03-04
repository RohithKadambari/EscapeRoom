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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (levelWiseDatas.Count == 0)
        {
            levelWiseDatas.Add(new LevelWiseData { maxInventoryItems = 10 });
            Debug.LogWarning("No level data found. Added default level data with max 10 inventory items.");
        }
    
    }

  
    public List<CollectableItem> inventoryItems = new List<CollectableItem>();

      public int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel",0);
        // Either initialize to 0:
        currentLevel = 0;

        // Or ensure the value from PlayerPrefs is valid:
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);

        // Make sure currentLevel is within range
        if (currentLevel >= levelWiseDatas.Count)
        {
            currentLevel = 0;
            Debug.LogWarning("CurrentLevel was out of range, reset to 0");
        }
    }
    

    public void AddItemsInInventory(string nameOfInventory, int quantity)
    {
        // First check if currentLevel is valid
        if (currentLevel < 0 || currentLevel >= levelWiseDatas.Count)
        {
            Debug.LogError(
                $"Current level {currentLevel} is out of range. LevelWiseDatas count: {levelWiseDatas.Count}");
            return;
        }

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