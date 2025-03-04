using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator; // Animator for the door
    private bool isOpen = false; // To track if the door is open or closed

    // You can modify this based on your input method (e.g., keyboard, mouse, or triggers)
    public KeyCode toggleKey = KeyCode.E; // Default key to toggle door state

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

}
