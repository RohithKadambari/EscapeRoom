using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private bool isFirstBatteryUse = true;

    private void Start()
    {
        offLight = false;
        canDrain = true;
        isBlinking = false;
        isFirstBatteryUse = true;
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
            if (!HasBatteryInInventory())
            {
                Debug.Log("No batteries in inventory!");
                return;
            }

            if (offLight)
                Invoke(nameof(FlashlightOff), 0.2f);
            else
                Invoke(nameof(FlashlightOn), 0.5f);
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
                StartCoroutine(BlinkingFlashlight());
            }
        
            if (batteryCapacity <= 0)
            {
                isBlinking = false;
                FlashlightOff();
                ConsumeBatteryFromInventory();
            }
        }
    }

    IEnumerator BlinkingFlashlight()
    {
        while (isBlinking)
        {
            lightComponent.enabled = false;
            yield return new WaitForSeconds(0.12f);
            lightComponent.enabled = true;
            yield return new WaitForSeconds(0.12f);
            lightComponent.enabled = false;
        }
    }

    bool HasBatteryInInventory()
    {
        var batteryItem = InventoryManager.Instance.inventoryItems.Find(item => item.itemName == "Battery");
        return batteryItem != null && batteryItem.itemQuantity > 0;
    }

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

            // Stop the blinking coroutine properly
            StopCoroutine(BlinkingFlashlight());
            isBlinking = false;
            lightComponent.enabled = true; // Make sure light is on after stopping blinking

            // Reset battery capacity
            batteryCapacity = 100;
            if (isFirstBatteryUse)
            {
                isFirstBatteryUse = false;
            }

            // Keep the inverse relationship for the slider
            batterySlider.value = 0f;

            if (HasBatteryInInventory())
            {
                var remainingBatteryItem =
                    InventoryManager.Instance.inventoryItems.Find(item => item.itemName == "Battery");
                Debug.Log("Used a battery. Remaining: " + remainingBatteryItem.itemQuantity);
            }
            else
            {
                Debug.Log("Used a battery. No more batteries in inventory.");
            }
        }
    }
}