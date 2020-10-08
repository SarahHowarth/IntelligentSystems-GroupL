using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
//priv fields
    private Transform position;
    private int id;
    private List<Packages> packages = new ArrayList<Packages>();

//public const
    public DropPoint(Transform position, int id)
    {
        this.id = id;
        this.position = position;
    }

    //in case we want to be able to use the const without the position param

    public DropPoint(int id)
    {
        this.id = id;
        
    }

    // Methods

    public void DeliverPackagesHere(List<Packages> packages)
    {
        this.packages.addRange(packages);
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

    public int Position
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
