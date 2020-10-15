using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DeliveryAgent : MonoBehaviour
{
    private int agentID;
    //private World world; - can do world.instance() instead if needed?
    private List<GameObject> packages;//List<Package> packages;
    private VehicleType type;
    private Depot depot;
    private Transform currentLoc;
    private List<GameObject> route;
    private bool hasRoute;

    //basically the constructor, just unity style
    public void Setup(VehicleType aType, Depot aDepot)
    {
        type = aType;
        depot = aDepot;
        currentLoc = depot.Position;
    }

    public void SendConstraints()
    {
        ACLMessage message = new ACLMessage();
        //assert contraints 
        
        //send to master routing agent via acl
    }

    public void RequestRoute()
    {
        //indicate via ACL
    }

    public void ReceiveRoute(ACLMessage message)
    {

    }

    public void DeliverPackages(Transform location)
    {
        foreach (GameObject p in packages)
        {
            DropPoint destination = p.GetComponent<Package>().Destination;
            if (destination.Position == location)
            {
                //deliver package to destination
                destination.DeliverPackageHere(p);
                //remove packpage from agent
                packages.Remove(p);
            }          
        }
    }

    public void MoveToNextLocation()
    {

    }

    public Transform GetLocation()
    {
        return currentLoc;
    }

    // Update is called once per frame
    void Update()
    {

        if(hasRoute)
        {
            MoveToNextLocation();
            DeliverPackages(currentLoc);
        }
        else
        {
            RequestRoute();
        }
        
    }
}
