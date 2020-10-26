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
    private bool paused;
    private LineRenderer lr;

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (hasRoute)
            {
                MoveToNextLocation();
                DeliverPackages(currentLoc);
            }
        }
    }

    //basically the constructor, just unity style
    public void Setup(VehicleType aType, Depot aDepot)
    {
        type = aType;
        depot = aDepot;
        currentLoc = depot.Position;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    /// <summary>
    /// Draw the route with a linerenderer
    /// </summary>
    /// <param name="route">the route list of gameobjects</param>
    public void DrawRoute(List<GameObject> route)
    {
        lr.positionCount = route.Count;

        for (int i = 0; i < route.Count; i++)
        {
            lr.SetPosition(i, route[i].transform.position);
        }

        lr.SetPosition(route.Count, route[0].transform.position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
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

    /// <summary>
    /// 
    /// </summary>
    public void MoveToNextLocation()
    {

    }

    /// <summary>
    /// ACL functions
    /// </summary>
    public void ReceiveConstraintRequest(ACLMessage message) 
    {
        //check I am the receiver

        //check depot is the sender

        //check performative
        if (message.Performative == "request constraints") 
        {
            SendConstraints();
        }
    }

    public void SendConstraints()
    {
        ACLMessage message = new ACLMessage();
        //assert contraints 
        
        //send to master routing agent via acl
    }

    public void ReceiveRoute(ACLMessage message)
    {

    }

    public void ReceivePackages(ACLMessage message)
    {

    }

    /// <summary>
    /// Properties 
    /// </summary>
    /// <returns></returns>
    public Transform GetLocation()
    {
        return currentLoc;
    }

    public int ID 
    { 
        get { return agentID; }
        set { agentID = value; }
    }

    //only for the WorldController setup
    public int GetWeight 
    { 
        get { return (int)type; } 
    }
}
