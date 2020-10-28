using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    private int id;
    private List<GameObject> packages = new List<GameObject>();

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
}
