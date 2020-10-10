using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Package : MonoBehaviour
{
    private int id;
    private float weight;
    private DropPoint destination;

    //use this for any animation type stuff
    //move package to be a child of this gameobject instead so it's not still floating around with the DA
    public void GetDelivered(Transform parentTransform) 
    {
        transform.SetParent(parentTransform, false);
    }

    public DropPoint Destination
    {
        get
        {
            return destination;
        }
        set
        {
            destination = value;
        }
    }

    public int ID
    {
        get
        {
            return id;
        }
        set 
        {
            id = value;
        }
    }

    public float Weight
    {
        get
        {
            return weight;
        }
        set
        {
            weight = value;
        }
    }
}
