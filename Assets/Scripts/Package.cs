using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Package : MonoBehaviour
{
    private int id;
    private float weight;
    private DropPoint destination;

    //use properties to set and get package
    //monobehaviour does not have default constructor 
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
