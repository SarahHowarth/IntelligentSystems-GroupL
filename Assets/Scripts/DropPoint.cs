using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    //priv fields
    private Transform position;
    private int id;
    private List<GameObject> packages = new List<GameObject>();

    private void Start()
    {
        //retrieve the position when the droppoint is spawned 
        //will set ID on spawn with property
        position = this.transform;
    }

    // Methods
    public void DeliverPackageHere(GameObject package)
    {
        //add package to list
        packages.Add(package);
        //tell package to get delivered - does unity stuff and/or any animations associated with the package
        package.GetComponent<Package>().GetDelivered(this.transform);
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
