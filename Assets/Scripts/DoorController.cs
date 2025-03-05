using System.Linq;
using System.Reflection;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public static DoorController instance;

    

    private Animator doorAnimator; // Animator for the door
    private bool isOpen = false; // To track if the door is open or closed

    // You can modify this based on your input method (e.g., keyboard, mouse, or triggers)
    public KeyCode toggleKey = KeyCode.E; // Default key to toggle door state

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Get the Animator component attached to the door
        doorAnimator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        // Toggle the door state and update the animation
        isOpen = !isOpen;

        if (isOpen)
        {
            // Play the door opening animation (you can change "Open" to your actual animation name)
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            // Play the door closing animation (you can change "Close" to your actual animation name)
            doorAnimator.SetTrigger("Close");
        }
    }

    public void DoorOpenWithKeys()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No inventory manageer found in the scene");
            return;
        }
        else
        {
            bool hasKey = InventoryManager.Instance.inventoryItems.Any(item => item.itemName.Contains("Key") && item.itemQuantity > 0);

            if (hasKey)
            {
                ToggleDoor();

                var KeyChecking = InventoryManager.Instance.inventoryItems.Find(item => item.itemName.Contains("Key"));

                if (KeyChecking != null)
                {
                    KeyChecking.itemQuantity--;

                    if (KeyChecking.itemQuantity <= 0)
                    {
                        InventoryManager.Instance.inventoryItems.Remove(KeyChecking);
                    }
                }
            }
        }
    }

}
