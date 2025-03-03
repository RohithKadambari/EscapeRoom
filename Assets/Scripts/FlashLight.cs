using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

public class FlashLight : MonoBehaviour
{
    public GameObject flashlight;
    public float midpointBatteryDrain;
    public float batteryCapacity;
    public float batteryDrain;
    public float lowestCapacity;
    public Slider batterySlider;
    private Light lightComponent;
    private bool offLight;
    private bool canDrain;
    private bool isBlinking;
    private Coroutine blinkCoroutine;
    
    

    private void Start()
    {
        offLight = false;
        canDrain = true;
        isBlinking = false;
        batteryCapacity = 100;
        lightComponent = flashlight.GetComponent<Light>();
        lightComponent.enabled = false;
        lightComponent.intensity = 3f;
    }

    private void Update()
    {
        FlashlightEnergyDrain();
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!offLight && HasBatteryInInventory())
            {
                Invoke(nameof(FlashlightOn), 0.5f);
            }
            else if (offLight)
            {
                Invoke(nameof(FlashlightOff), 0.2f);
            }
            else if (!HasBatteryInInventory())
            {
                Debug.Log("No batteries in inventory!");
            }
        }
    }

    void FlashlightOn()
    {
        lightComponent.enabled = true;
        offLight = true;
        canDrain = true;
    }

    void FlashlightOff()
    {
        lightComponent.enabled = false;
        offLight = false;
        canDrain = false;
        
        // If we're blinking, stop the coroutine
        if (isBlinking && blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            isBlinking = false;
        }
    }
    
    // Check if player has batteries in inventory
    bool HasBatteryInInventory()
    {
        var batteryItem = InventoryManager.Instance.inventoryItems.Find(item => item.itemName == "Battery");
        return batteryItem != null && batteryItem.itemQuantity > 0;
    }
    
    // Consume a battery from inventory and reset flashlight charge
    void ConsumeBatteryFromInventory()
    {
        if (HasBatteryInInventory())
        {
            var batteryItem = InventoryManager.Instance.inventoryItems.Find(item => item.itemName == "Battery");
            batteryItem.itemQuantity--;

            if (batteryItem.itemQuantity <= 0)
            {
                InventoryManager.Instance.inventoryItems.Remove(batteryItem);
            }

            batteryCapacity = 100;

            // Update the UI slider to full (battery is full)
            batterySlider.value = 1f;

            // Log the remaining battery count
            if (HasBatteryInInventory())
            {
                var remainingBatteryItem = InventoryManager.Instance.inventoryItems.Find(item => item.itemName == "Battery");
                Debug.Log("Used a battery. Remaining: " + remainingBatteryItem.itemQuantity);
            }
            else
            {
                Debug.Log("Used a battery. No more batteries in inventory.");
            }

            if (!offLight)
            {
                FlashlightOn();
            }
        }
    }

    void FlashlightEnergyDrain()
    {
        if (offLight && canDrain)
        {
            batteryCapacity -= batteryDrain * Time.deltaTime; 
            batterySlider.value = 1f - (batteryCapacity / 100f); 

            if (batteryCapacity < midpointBatteryDrain && batteryCapacity >= lowestCapacity)
            {
                lightComponent.intensity = 2.5f;
            }
            else if (batteryCapacity >= 50)
            {
                lightComponent.intensity = 3f;
            }
            else if (batteryCapacity < lowestCapacity && !isBlinking)
            {
                isBlinking = true;
                blinkCoroutine = StartCoroutine(BlinkingFlashlight());
            }

            if (batteryCapacity <= 0)
            {
                batteryCapacity = 0; 
                isBlinking = false;
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }

                FlashlightOff(); 

                if (HasBatteryInInventory())
                {
                    ConsumeBatteryFromInventory(); 
                    if (batteryCapacity > 0)
                    {
                        FlashlightOn(); 
                    }
                }
                else
                {
                    Debug.Log("Flashlight battery depleted and no more batteries in inventory!");
                }
            }
        }
    }


    IEnumerator BlinkingFlashlight()
    {
        while (isBlinking && batteryCapacity > 0)
        {
            Debug.Log("Flashlight blinking... Battery low!");
            lightComponent.enabled = false;
            yield return new WaitForSeconds(0.12f);
            lightComponent.enabled = true;
            yield return new WaitForSeconds(0.12f);
        }
        
        if (offLight && batteryCapacity > 0)
        {
            lightComponent.enabled = true;
        }
        else
        {
            lightComponent.enabled = false;
        }
    }
}