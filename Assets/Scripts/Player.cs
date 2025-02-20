using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    int keys = 0;
    private Rigidbody PlayerRB;
    [SerializeField] int speed;
    public Transform playerBody;

    
    

    public float mouseSensitivity;
    public float xRotation = 0;

   

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovements();
        MouseControl();
        
    }

    public void PlayerMovements()
    {
        var Horizontal=Input.GetAxisRaw("Horizontal");
        var Vertical=Input.GetAxisRaw("Vertical");
        PlayerRB.linearVelocity = new Vector3(Horizontal * speed , PlayerRB.linearVelocity.y , Vertical * speed);
      
    }
    void MouseControl(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity* Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

         xRotation -=mouseY;
         xRotation=Mathf.Clamp(xRotation,-90f,90f);

        transform.localRotation = Quaternion.Euler(xRotation,0f,0f);

        playerBody.Rotate(Vector3.up*mouseX);
    }


    /*void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("key")){
            keys++;
            
            Destroy(other.gameObject);
            if(keys>=5){
                Debug.Log("All 5 Keys are Collected");
            }
        }
        else{
            Debug.Log("collect keys first");
        }
    }*/
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("key"))
        {
            
            string name = other.gameObject.tag;
            keys++;
            InventoryManager.instance.AddItemsToInventory(name,1);
            if (InventoryManager.instance.canCollect == true)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
