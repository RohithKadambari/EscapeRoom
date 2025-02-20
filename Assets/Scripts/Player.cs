using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    int keys = 0;
    private Rigidbody PlayerRB;
    [SerializeField] int speed;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovements();
    }

    public void PlayerMovements()
    {
        var Horizontal=Input.GetAxisRaw("Horizontal");
        var Vertical=Input.GetAxisRaw("Vertical");
        PlayerRB.velocity = new Vector3(Horizontal * speed , PlayerRB.velocity.y , Vertical * speed);
      
    }


    void OnCollisionEnter(Collision other)
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
    }
}
