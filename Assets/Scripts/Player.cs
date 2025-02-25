using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    int keys = 0;
    private Rigidbody playerRB;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;

    public Transform cameraTransform;

    private float xRotation = 0f;


    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MouseControl();
    }

    void FixedUpdate()
    {
        PlayerMovements();
    }

    void PlayerMovements()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.Normalize();


        playerRB.MovePosition(playerRB.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void MouseControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;


        transform.Rotate(Vector3.up * mouseX);


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void crosshairInteractables()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 50f))
        {
            if (hit.collider.tag == "Interactable")
            {
                //InteractionText--TextMeshProObject variable creation set active true hit.collider.gameobject.name
            }
            else
            {
                //InteractionText setactive false
            }
        }
        else
        {
            //InteractionText false 
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Key"))
        {

            string name = other.gameObject.tag;
            keys++;
            InventoryManager.Instance.AddItemsInInventory(name, 1);
            Destroy(other.gameObject);

        }
        if (other.gameObject.CompareTag("Battery"))
        {
            string name = other.gameObject.tag;
            var batteryItem = InventoryManager.Instance.inventoryItems.Find(p => p.itemName == "Battery");
            if ((batteryItem == null) || (batteryItem.itemQuantity < InventoryManager.Instance.GetMaxCapcityFor(name)))
            {
                Debug.Log("Enter the dragon");
                InventoryManager.Instance.AddItemsInInventory(name, 1);

            }
            else
            {
                Debug.Log("Battery max capacity is reached ");
            }
        }
    }

}

