using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure we use UI namespace instead of UIElements

[System.Serializable]
public class LevelWiseData {
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
        public Sprite icon; // Changed to Sprite for UI

        public CollectableItem(string itemName, int itemQuantity, Sprite icon = null)
        {
            this.itemName = itemName;
            this.itemQuantity = itemQuantity;
            this.icon = icon;
        }
    }

    [System.Serializable]
    public class ItemMax
    {
        public string itemName;
        public int maxCapacity;
        public Sprite icon; // Added icon field to store default icon
    }

    [System.Serializable]
    public class ItemUIPanel
    {
        public string itemType; // e.g., "key", "battery"
        public GameObject panelObject;
        public Image iconImage;
        public TMPro.TextMeshProUGUI quantityText; // For showing quantity
    }

    public List<LevelWiseData> levelWiseDatas;
    public List<ItemMax> maxItems;
    public List<ItemUIPanel> itemUIPanels; 

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
        // Initialize currentLevel
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);

        // Make sure currentLevel is within range
        if (currentLevel >= levelWiseDatas.Count)
        {
            currentLevel = 0;
            Debug.LogWarning("CurrentLevel was out of range, reset to 0");
        }

        foreach (var panel in itemUIPanels)
        {
            panel.panelObject.SetActive(false);
        }
    }

    public void AddItemsInInventory(string nameOfInventory, int quantity)
    {
        // First check if currentLevel is valid
        if (currentLevel < 0 || currentLevel >= levelWiseDatas.Count)
        {
            Debug.LogError($"Current level {currentLevel} is out of range. LevelWiseDatas count: {levelWiseDatas.Count}");
            return;
        }

        var itemInfo = maxItems.Find(item => item.itemName == nameOfInventory);
        Sprite icon;
        if (itemInfo != null)
        {
            icon = itemInfo.icon;
        }
        else
        {
            icon = null;
        }
        
        var existingInventoryItem = inventoryItems.Find(item => item.itemName == nameOfInventory);

        if (existingInventoryItem != null)
        {
            // Check if adding more would exceed max capacity
            int maxCapacity = GetMaxCapcityFor(nameOfInventory);
            if (maxCapacity > 0 && existingInventoryItem.itemQuantity + quantity > maxCapacity)
            {
                existingInventoryItem.itemQuantity = maxCapacity;
                Debug.Log($"{nameOfInventory} is at max capacity: {maxCapacity}");
            }
            else
            {
                existingInventoryItem.itemQuantity += quantity;
            }
        }
        else if (inventoryItems.Count < levelWiseDatas[currentLevel].maxInventoryItems)
        {
            inventoryItems.Add(new CollectableItem(nameOfInventory, quantity, icon));
        }
        else
        {
            Debug.Log("Inventory is full");
            return; 
        }

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        foreach (var panel in itemUIPanels)
        {
            panel.panelObject.SetActive(false);
        }

        foreach (var item in inventoryItems)
        {
            var panel = itemUIPanels.Find(p => p.itemType == item.itemName);
            
            if (panel != null && item.itemQuantity > 0)
            {
                panel.panelObject.SetActive(true);
                
                if (panel.iconImage != null && item.icon != null)
                {
                    panel.iconImage.sprite = item.icon;
                    panel.iconImage.enabled = true;
                }
                
                if (panel.quantityText != null && item.itemQuantity > 1)
                {
                    panel.quantityText.text = item.itemQuantity.ToString();
                    panel.quantityText.gameObject.SetActive(true);
                }
                else if (panel.quantityText != null)
                {
                    panel.quantityText.gameObject.SetActive(false);
                }
            }
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
    
    public void RemoveItemFromInventory(string itemName, int quantity = 1)
    {
        var item = inventoryItems.Find(i => i.itemName == itemName);
        
        if (item != null)
        {
            item.itemQuantity -= quantity;
            
            if (item.itemQuantity <= 0)
            {
                inventoryItems.Remove(item);
            }
            
            UpdateInventoryUI();
        }
    }
}