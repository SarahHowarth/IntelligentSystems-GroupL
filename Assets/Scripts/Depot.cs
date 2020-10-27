using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Infrastructure.Framework.Threading;
using GeneticSharp.Domain.Selections;
using System.Threading;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Terminations;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class Depot :  MonoBehaviour
{
    private List<GameObject> deliveryAgents = new List<GameObject>();
    private List<GameObject> vehiclesAtDepot = new List<GameObject>();
    private Dictionary<int, float> truckCapacity = new Dictionary<int, float>();

    private List<GameObject> allPackages = new List<GameObject>();
    private List<GameObject> packagesAtDepot = new List<GameObject>();
    private List<GameObject> dropPoints = new List<GameObject>();

    private Dictionary<int, List<GameObject>> routes = new Dictionary<int, List<GameObject>>(); 

    private Transform position;

    private GeneticAlgorithm ga;
    private readonly int NUMBER_OF_GENERATIONS = 300;
    private int constraintsReceived;

    private void Start()
    {
        position = this.gameObject.transform;
    }

    /// <summary>
    /// Called by the world controller to setup all the data 
    /// </summary>
    /// <param name="dPoints"></param>
    /// <param name="dAgent"></param>
    /// <param name="minPackages"></param>
    /// <param name="maxPackages"></param>
    public void Setup(List<GameObject> dPoints, List<GameObject> dAgent, List<GameObject> packages)
    {
        //clear just incase
        routes.Clear();
        allPackages.Clear();
        packagesAtDepot.Clear();
        dropPoints.Clear();
        deliveryAgents.Clear();
        vehiclesAtDepot.Clear();
        truckCapacity.Clear();

        //setup dropoints
        dropPoints = dPoints;
        //setup packages
        allPackages = packages;
        packagesAtDepot = packages;
        //setup delivery agents
        deliveryAgents = dAgent;
        vehiclesAtDepot = dAgent;

        //request constraints from delivery agents after setup
        constraintsReceived = 0;
        RequestConstraints();
    }
    
    //Genetic algorithm VRP runner
    private void StartGA() 
    {
        var fitness = new VRPFitness(this);
        var chromosome = new VRPChromosome(dropPoints.Count, deliveryAgents.Count);
        var crossover = new OrderedCrossover();
        var mutation = new ReverseSequenceMutation();
        var selection = new RouletteWheelSelection();
        var population = new Population(50, 100, chromosome);

        ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
        ga.Termination = new GenerationNumberTermination(NUMBER_OF_GENERATIONS);
        UIController.Instance.NumberGenerations(NUMBER_OF_GENERATIONS);

        ga.GenerationRan += delegate
        {
            double totalDemand = 0;
            double totalDistance = 0;
            Debug.Log($"Generation: {ga.GenerationsNumber} - Fitness: ${ga.BestChromosome.Fitness}");
            var c = ga.Population.CurrentGeneration.BestChromosome as VRPChromosome;
            for (int i = 0; i < deliveryAgents.Count; i++)
            {
                double distance = fitness.CalcTotalDistance(i, c);
                double demand = fitness.CalcTotalDemand(i, c);
                Debug.Log("vehicle ID: " + i.ToString() + ", distance: " + distance.ToString() + ", demand: " + demand.ToString());
                totalDemand += demand;
                totalDistance += distance;
            }
            UIController.Instance.SetFitness(totalDemand, totalDistance);
        };

        ga.TerminationReached += delegate
        {
            Debug.Log("GA done");
            GetBestRoutes(ga.Population.CurrentGeneration.BestChromosome as VRPChromosome, fitness);
        };

        ga.Start();
    }

    //gets the best routes once the GA is done
    private void GetBestRoutes(VRPChromosome c, VRPFitness f) 
    {
        if (c != null)
        {
            Dictionary<int, List<int>> routesNotFormatted = new Dictionary<int, List<int>>();
            for (int i = 0; i < deliveryAgents.Count; i++) 
            {
                List<int> route = f.GetPositions(i, c);
                FormatRoute(i, route);
                double distance = f.CalcTotalDistance(i, c);
                double demand = f.CalcTotalDemand(i, c);
                Debug.Log("vehicle ID: " + i.ToString() + ", distance: " + distance.ToString() + ", demand: " + demand.ToString());
            }
            SendRoutes();
        }
    }

    //formats the route ID's to a route of droppoint gameobjects 
    private void FormatRoute(int vehicleID, List<int> nRoute) 
    {
        List<GameObject> formattedRoute = new List<GameObject>();
        foreach (int ID in nRoute) 
        {
            foreach (GameObject g in dropPoints) 
            {
                DropPoint dp = g.GetComponent<DropPoint>();
                if (dp.ID == ID) 
                {
                    formattedRoute.Add(g);
                }
            }
        }
        //adding depot as last object in the route so they return here
        formattedRoute.Add(this.gameObject);
        routes.Add(vehicleID, formattedRoute);
    }

    /// <summary>
    /// ACL functions
    /// </summary>
    private void RequestConstraints() 
    {
        ACLMessage constraintRequest = new ACLMessage();
        constraintRequest.Sender = "depot";
        //request constraints from every delivery agent 
        foreach (GameObject g in deliveryAgents)
        {
            DeliveryAgent dA = g.GetComponent<DeliveryAgent>();
            constraintRequest.Receiver = "delivery agent:" + dA.ID.ToString();
            constraintRequest.Performative = "request constraints";
            constraintRequest.Content = "";//nothing to send just a request 
            g.SendMessage("ReceiveConstraintRequest", constraintRequest);
        }
    }

    public void ReceiveConstraints(ACLMessage message)
    {
        //check it's for me
        if (message.Receiver != "depot") 
        { 
            return; 
        }

        //check it's from delivery agent
        string[] splitString = message.Sender.Split(':');
        if (splitString[0] != "delivery agent") 
        {
            return;
        }

        //check performative
        //assign to truck capacity dictionary based on sender id 
        if (message.Performative == "send constraints") 
        {
            int agentID = int.Parse(splitString[1]);
            float agentCapacity = float.Parse(message.Content);
            truckCapacity[agentID] = agentCapacity;
            constraintsReceived += 1;
        }

        if (constraintsReceived == deliveryAgents.Count) 
        {
            StartGA();
        }
    }

    public void SendRoutes()
    {
        //construct the ACL message and send the route to the DA 
        ACLMessage routeMessage = new ACLMessage();
        routeMessage.Sender = "depot";
        foreach (GameObject g in deliveryAgents)
        {
            DeliveryAgent dA = g.GetComponent<DeliveryAgent>();
            routeMessage.Receiver = "delivery agent:" + dA.ID.ToString();
            routeMessage.Performative = "send route";
            routeMessage.GameObjectContent = routes[dA.ID];
            SendPackages(dA.ID, g, routes[dA.ID]);
            g.SendMessage("ReceiveRoute", routeMessage);
            vehiclesAtDepot.Remove(g);
        }
    }

    private void SendPackages(int agentID, GameObject agentObject, List<GameObject> route)
    {
        List<GameObject> allocatedPackages = new List<GameObject>();
        //construct the ACL message and send the packages to the DA 
        ACLMessage packageMessage = new ACLMessage();
        packageMessage.Sender = "depot";
        packageMessage.Receiver = "delivery agent:" + agentID.ToString();
        packageMessage.Performative = "send packages";
        //will need to child the package game object to the truck 
        for(int i = 0; i < route.Count-1; i++) //last point is the depot so will get error if get component drop point
        {
            DropPoint dp = route[i].GetComponent<DropPoint>();
            foreach (GameObject objectPackage in allPackages) 
            {
                Package p = objectPackage.GetComponent<Package>();
                if (p.Destination = dp) 
                {
                    objectPackage.transform.SetParent(agentObject.transform, false);
                    allocatedPackages.Add(objectPackage);
                    packagesAtDepot.Remove(objectPackage);
                }
            }
        }
        packageMessage.GameObjectContent = allocatedPackages;
        agentObject.SendMessage("ReceivePackages", packageMessage);
    }

    /// <summary>
    /// Properties
    /// </summary>
    public Transform Position 
    {
        get { return position; }
        set { position = value; }
    }

    public List<GameObject> DeliveryAgents 
    { 
        get { return deliveryAgents; }
    }

    public List<GameObject> DropPoints
    {
        get { return dropPoints; }
    }

    public float GetVehicleCapacity(int ID)
    {
        return truckCapacity[ID];
    }

    public float GetDropPointDemand(DropPoint dp)
    {
        float dropPointDemand = 0.0f;

        foreach (GameObject p in allPackages)
        {
            Package thePackage = p.GetComponent<Package>();
            if (thePackage.Destination == dp)
            {
                dropPointDemand += thePackage.Weight;
            }
        }
        return dropPointDemand;
    }
}
