using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class DeliveryAgent : MonoBehaviour
{
    private int agentID;
    private List<GameObject> packages;//List<Package> packages;
    private VehicleType type;
    private Depot depot;
    private List<GameObject> route;
    private bool hasRoute;
    private bool paused;
    [SerializeField]private LineRenderer lineRendererComponent = default;

    public float speed = 2.0f;
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (hasRoute)
            {
                if (MoveToNextLocation())
                    DeliverPackages();
            }
        }
    }

    //basically the constructor, just unity style
    public void Setup(VehicleType aType, Depot aDepot)
    {
        type = aType;
        depot = aDepot;
        rb = GetComponent<Rigidbody>();
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
        lineRendererComponent.positionCount = route.Count;

        for (int i = 0; i < route.Count; i++)
        {
            lineRendererComponent.SetPosition(i, route[i].transform.position);
        }

        lineRendererComponent.SetPosition(route.Count, route[0].transform.position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    public void DeliverPackages()
    {
        foreach (GameObject p in packages)
        {
            DropPoint destination = p.GetComponent<Package>().Destination;
            //check if within distance threshold
            if (Vector3.Distance(destination.Position.position, transform.position) <= 0.5)
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
    public bool MoveToNextLocation()
    {
        //return true if destination reached
        if (Vector3.Distance(packages[0].transform.position, transform.position) <= 0.5)
        {
            return true;
        }

        //difference in target and self positions to get direction of next point
        Vector3 diff = packages[0].transform.position - transform.position;

        //get direction of next point, normalise and multiply by speed for directed movement vector
        Vector3 dir = diff.normalized * speed;

        //Set the direction the vehicle faces
        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(diff, Vector3.up);
        transform.rotation = rotation;

        //may need to be rb.vector = dir;
        rb.MovePosition(dir);
        return false;


        
    }

    /// <summary>
    /// ACL functions
    /// </summary>
    public void ReceiveConstraintRequest(ACLMessage message) 
    {
        //check send/recievers
        if (!CheckAddresses(message))
            return;

        //check performative
        if (message.Performative == "request constraints") 
        {
            SendConstraints(message);
        }
    }

    public void SendConstraints(ACLMessage request)
    {
        //create message with constraint content.
        ACLMessage constraintMessage = new ACLMessage();
        constraintMessage.Sender = request.Receiver;
        constraintMessage.Receiver = request.Sender;
        constraintMessage.Performative = "send constraints";

        //assert contraints (cast to int to avoid sending "truck" etc.)
        int content = (int) type;
        constraintMessage.Content = content.ToString();

        //send to master routing agent via acl
        depot.SendMessage("ReceiveConstraints", constraintMessage);
    }

    public void ReceiveRoute(ACLMessage message)
    {
        //check send/recievers
        if (!CheckAddresses(message))
            return;

        //assert that the message is the route information
        if(message.Performative != "send route")
            return;

        //populate route
        route = message.GameObjectContent;

    }

    public void ReceivePackages(ACLMessage message)
    {
        //check send/recievers
        if (!CheckAddresses(message))
            return;

        //assert that the message is package gameObjects
        if (message.Performative != "send packages")
            return;

        //load packages to DA
        packages = message.GameObjectContent;
    }

    public bool CheckAddresses(ACLMessage message)
    {
        //check its for me
        if (message.Receiver != "delivery agent:" + ID.ToString())
        {
            return false;
        }

        //check it's from the Depot
        if (message.Sender != "depot")
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Properties 
    /// </summary>
    /// <returns></returns>

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
