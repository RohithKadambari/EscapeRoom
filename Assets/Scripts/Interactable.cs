using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfObject
{
    Key,
    Battery,
    Door,

}



public class Interactable : MonoBehaviour
{
    public static Interactable instance;

    public TypeOfObject typeOfObject;
    public List<TypeOfObject> typeOfObjects = new List<TypeOfObject>();

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
