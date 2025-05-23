using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    int keys = 0;
    private Rigidbody playerRB;

    bool door = false;
    bool keyfordoor = false;


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;



    public Transform cameraTransform;

    [SerializeField] GameObject Door;
    bool DoorClosed;

    private float xRotation = 0f;

    public float bounceFrequency = 2f;

    public float bounceAmplitude = 0.1f;

    Vector3 OriginalPos;

    float SphereRadius = 10f;

    [SerializeField] LayerMask InteractableLayer;

    bool CanyouStand;




    void Start()
    {
        CanyouStand = true;
        playerRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        OriginalPos = cameraTransform.localPosition;

    }

    void Update()
    {
        MouseControl();
        crosshairInteractables();

        if (door)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DoorController.instance.ToggleDoor();

            }
        }


    }

    void FixedUpdate()
    {
        PlayerMovements();

    }

    void PlayerMovements()
    {
        if (CanyouStand)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            playerRB.velocity = moveDirection * moveSpeed;
            if (moveDirection.magnitude >= 0f)
            {
                Walkbounce();
            }
        }
        else
        {
            CanyouStand = false;
        }
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

    public void Walkbounce()
    {
        float bounce = Mathf.Sin(Time.time * bounceFrequency) * bounceAmplitude;
        cameraTransform.localPosition = OriginalPos + (Vector3.up * bounce);
    }

    public void crosshairInteractables()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 1000f, InteractableLayer))
        {
            hit.collider.TryGetComponent<Interactable>(out Interactable it);
            if (it != null)
            {
                Debug.Log("Hitting.. " + it.typeOfObject);
            }
        }
        else
        {
            //InteractionText false 
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * 100f);
    }

    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Battery"))
        {
            string name = other.gameObject.tag;
            var batteryItem = InventoryManager.Instance.inventoryItems.Find(p => p.itemName == "Battery");

            if ((batteryItem == null) || (batteryItem.itemQuantity < InventoryManager.Instance.GetMaxCapcityFor(name)))
            {
                Debug.Log("Got the battery");
                InventoryManager.Instance.AddItemsInInventory(name, 1);
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("Battery max capacity is reached ");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            door = true;

            if (Input.GetKey(KeyCode.E) && DoorClosed)
            {
                Debug.Log("player");
                Door.GetComponent<Animation>().Play("DoorOpening");
                DoorClosed = false;
            }
            else if (Input.GetKey(KeyCode.E) && !DoorClosed)
            {
                Door.GetComponent<Animation>().Play("DoorClosing");
                DoorClosed = true;
            }
            }

        if (other.gameObject.CompareTag("Key"))
        {
            keyfordoor = true;

            if (Input.GetKey(KeyCode.E) && keyfordoor)
            {
                string name = other.gameObject.tag;
                keys++;
                InventoryManager.Instance.AddItemsInInventory(name, 1);
                Destroy(other.gameObject);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            door = false;
        }
    }

}

