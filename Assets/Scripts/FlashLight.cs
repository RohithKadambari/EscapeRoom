using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class FlashLight : MonoBehaviour
{
    public GameObject flashlight;
    public float midpointBatteryDrain;
    public float batteryCapacity;
    public float batteryDrain;
    public float lowestCapacitry;
    public Slider batterySlider;
    private Light lightComponent;
    private bool offLight;
    private bool canDrain;
    private bool isBlinking;
    

    private void Start()
    {
        
        offLight = false;
        canDrain = true;
        isBlinking = false;
        batteryCapacity = 100;
        midpointBatteryDrain = 50f;
        lowestCapacitry = 20f;
        lightComponent = flashlight.GetComponent<Light>();
        lightComponent.enabled = false;
        lightComponent.intensity = 3f;
    }

    private void Update()
    {
        FlashlightEnergyDrain();
        if (Input.GetKeyDown(KeyCode.F))
        {
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
            batterySlider.value = 1f -(batteryCapacity/100f);

        if (batteryCapacity<50 && batteryCapacity>=10)
        { 
            lightComponent.intensity = 2.5f;
        }
        else if(batteryCapacity>=50)
        {
            lightComponent.intensity = 3f;
        }
        else if (batteryCapacity < 10 && !isBlinking)
        {
            
            isBlinking = true;
            StartCoroutine(BlinkingFlashlight());

        }
        /*
        if (batterySlider.value < lowestCapacitry/100f && !isBlinking)
        {
            StartCoroutine(BlinkingFlashlight());
        }
        else if (batterySlider.value > lowestCapacitry/100f && isBlinking)
        {
            StopCoroutine(BlinkingFlashlight());
            lightComponent.enabled = true;
            isBlinking = false;
        }
        */

        if (batteryCapacity <= 0)
        {
            FlashlightOff();
            isBlinking = false;
            lightComponent.enabled = false;
        }
        
        }
        
    }

    IEnumerator BlinkingFlashlight()
    {
        while (isBlinking)
        {
            Debug.Log("Entering here...");
            lightComponent.enabled = false;
            yield return new WaitForSeconds(0.12f);
            lightComponent.enabled = true;
            yield return new WaitForSeconds(0.12f);
        }
    }
}