﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Google.OrTools;
using UnityEngine;
using TMPro.EditorUtilities;

/// <summary>
/// This ia an abstract class for the depot to implement 
/// </summary>
public class MasterRoutingAgent : MonoBehaviour
{
    protected List<GameObject> deliveryAgents;
    protected List<GameObject> vehiclesAtDepot;
    protected Dictionary<GameObject, int> truckCapacity;

    protected List<GameObject> allPackages;
    protected List<GameObject> packagesToDeliver;

    protected List<GameObject> dropPoints;


    //or tools data
    protected long[,] distanceMatrix;
    protected int numVehicles;

    public void Setup() {
       //set the intial variables by looping through and adding the actual delivery agent scripts to the lists
       //world controller keeps a reference of the gameobjects while the masterrouting agent holds the actual agent scripts
       
       //setup packages to deliver and droppoints
    }

    public void SetupVRP() { }

    public void StartVRP() {  }

    public void ReceiveConstraints(ACLMessage message) 
    { 
       //will be called by delivery agent
       //assign to truck capacity dictionary based on sender id 
    }

    public void ReceiveRouteRequest(ACLMessage message) 
    { 
        //will be called by delivery agent 
        //calculate route when a delivery agent asks for one  
        //then send back 
    }

    public void SendRoute() 
    { 
        //construct the ACL message and send the route to the DA 
    }
}