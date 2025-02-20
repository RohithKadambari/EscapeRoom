using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Collectables{
    Keys,

}
public class InventoryManager : MonoBehaviour
{
    public Collectables collectables;

   // public List<GameObject> Interactables=new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable")
        {
            

        }
    }
}
