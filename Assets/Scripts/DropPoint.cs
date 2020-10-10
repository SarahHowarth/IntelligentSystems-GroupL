using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    //priv fields
    private Transform position;
    private int id;
    private List<Package> packages = new List<Package>();

    private void Start()
    {
        //retrieve the position when the droppoint is spawned 
        //will set ID on spawn with property
        position = this.transform;
    }

    // Methods
    public void DeliverPackageHere(Package package)
    {
        packages.Add(package);
    }

    //public props
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

    public Transform Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

}
