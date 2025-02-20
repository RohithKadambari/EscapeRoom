using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    int keys = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
