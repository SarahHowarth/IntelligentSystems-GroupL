using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DeliveryAgent : MonoBehaviour
{
    private int agentID;
    private World world;
    private list<DropPoint> route;
    private list<Package> packages;
    private VehicleType type;
    private Depot depot;
    private transform currentLoc;


    DeliveryAgent(World aWorld, VehicleType aType, Depot aDepot)
    {
        world = aWorld;
        type = aType;

        depot = aDepot;
        currentLoc = depot.GetPosition();
    }

    public void SendConstraints()
    {
        ACLMessage message;



        MasterRoutingAgent.ReceiveConstraints(message);
    }

    public void RequestRoute(ACLMessage message)
    {

    }

    public bool RecieveRoute(ACLMessage message)
    {

    }

    public bool DeliverPackages(Transform Location)
    {
        bool result = false;
        foreach (int i in packages){
            if(i.GetDestination().GetPosition() == Location)
            {
                result = true;
            }
        }

        return result;
    }

    public bool MoveToNextLocation()
    {

    }


    public bool GetLocation()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
