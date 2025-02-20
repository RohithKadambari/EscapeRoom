using System.Collections;
using System.Collections.Generic;
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("key"))
        {
            keys++;
            Destroy(other.gameObject);
            if (keys >= 5)
            {
                Debug.Log("All 5 Keys are Collected");
            }
        }
        else
        {
            Debug.Log("Collect keys first");
        }
    }
}
