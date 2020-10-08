using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Package : MonoBehaviour
{
    private int id;
    private int weight;
    private DropPoint destination;

    public Package(int id, int weight)
    {

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

    public int getID
    {
        get
        {
            return id;
        }
    }

    public int getWeight
    {
        get
        {
            return weight;
        }
    }
}
