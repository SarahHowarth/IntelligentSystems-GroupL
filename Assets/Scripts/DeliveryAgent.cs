using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class DeliveryAgent : MonoBehaviour
{
    private int agentID;
    private List<GameObject> packages = new List<GameObject>();//List<Package> packages;
    private VehicleType type;
    private Depot depot;
    private List<GameObject> route = new List<GameObject>();
    private bool hasRoute = false;
    private bool paused;
    [SerializeField]private LineRenderer lineRendererComponent = default;

    public float speed = 50.0f;
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        if(route.Count == 0)
        {
            hasRoute = false;
        }
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
    private void DrawRoute()
    {
        Color c1 = new Color(UnityEngine.Random.Range(.3F, 1F), UnityEngine.Random.Range(.3F, 1F), UnityEngine.Random.Range(.3F, 1F));
        lineRendererComponent.startColor = c1;
        lineRendererComponent.endColor = c1;
        lineRendererComponent.positionCount = route.Count + 1;
        Vector3 depotPosition = new Vector3(0, 0, 0);

        //set start of line renderer
        lineRendererComponent.SetPosition(0, depotPosition);

        //draw route
        //end of route is already depot so don't need to add in end
        for (int i = 0; i < route.Count; i++)
        {
            lineRendererComponent.SetPosition(i + 1, route[i].transform.position);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    private void DeliverPackages()
    {
        foreach (GameObject p in packages)
        {
            DropPoint destination = p.GetComponent<Package>().Destination;
            //check if within distance threshold
            if (Vector3.Distance(destination.transform.position, transform.position) <= 0.5)
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
    private bool MoveToNextLocation()
    {
        //return true if destination reached
        if (Vector3.Distance(route[0].transform.position, transform.position) <= 0.5)
        {
            return true;
        }

        //difference in target and self positions to get direction of next point
        Vector3 diff = route[0].transform.position - transform.position;

        //get direction of next point, normalise and multiply by speed for directed movement vector
        Vector3 dir = diff.normalized * speed;

        //Set the direction the vehicle faces
        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(diff, Vector3.up);
        transform.rotation = rotation;

        //may need to be rb.MovePosition(dir); 
        //rb.velocity = dir;
        var change = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, route[0].transform.position, change);
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
        Debug.Log("Route list size: " + route.Count.ToString());
        hasRoute = true;
        DrawRoute();
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
        //for debugging
        string packDebug = packages.Count.ToString();
        Debug.Log("Size of packages array" + packDebug);
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
