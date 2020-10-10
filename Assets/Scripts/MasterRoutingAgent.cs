using System.Collections;
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
    protected Dictionary<int, float> truckCapacity;

    protected List<GameObject> allPackages;
    protected List<GameObject> packagesAtDepot;

    protected List<GameObject> dropPoints;


    //or tools data
    protected long[,] distanceMatrix;
    protected int numVehicles;

    public void Setup(List<GameObject> dPoints, List<GameObject> dAgent, List<GameObject> packages)
    {
        //setup packages to deliver and droppoints
        allPackages.Clear();
        packagesAtDepot.Clear();
        allPackages = packages;
        packagesAtDepot = packages;
        dropPoints.Clear();
        dropPoints = dPoints;

        //loop through droppoints and set ID's

        //loop through packages, set values and assign to a droppoint


        //setup delivery agents and their capacity
        deliveryAgents.Clear();
        vehiclesAtDepot.Clear();
        truckCapacity.Clear();
        deliveryAgents = dAgent;
        vehiclesAtDepot = dAgent;
        //loop through agents and set them up
    }

    public void ReceiveConstraints(ACLMessage message)
    {
        //will be called by delivery agent
        //assign to truck capacity dictionary based on sender id 
    }

    public void ReceiveRouteRequest(ACLMessage message)
    {
        //will be called by delivery agent 
        //assign packages and send back message  
        //calculate route and send back message 
    }

    public void SendPackages()
    {
        //construct the ACL message and send the assigned packages to the DA 
    }

    public bool SendRoute()
    {
        //construct the ACL message and send the route to the DA 
        return true;
    }

    private void AssignPackages(int agentID) 
    { 
        //used to calculate and assign packages to a truck
        
        //will need to child the package game object to the truck 

        //loop through and remove assigned packages from packagesAtDepot list 
    }
}
