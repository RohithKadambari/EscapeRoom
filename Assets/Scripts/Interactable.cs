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
        if (Interactable.instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
