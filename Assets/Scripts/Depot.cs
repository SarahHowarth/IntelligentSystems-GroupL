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
    private List<GameObject> deliveryAgents;
    private List<GameObject> vehiclesAtDepot;
    private Dictionary<int, float> truckCapacity;

    private List<GameObject> allPackages;
    private List<GameObject> packagesAtDepot;
    private List<GameObject> dropPoints;

    private Dictionary<int, List<GameObject>> routes; 

    private Transform position;
    private LineRenderer lr;

    private GeneticAlgorithm ga;
    private readonly int NUMBER_OF_GENERATIONS = 300;

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

    }

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

        ga.GenerationRan += delegate
        {
            Debug.Log($"Generation: {ga.GenerationsNumber} - Fitness: ${ga.BestChromosome.Fitness}");
        };

        ga.TerminationReached += delegate
        {
            Debug.Log("GA done");
            GetBestRoutes(ga.Population.CurrentGeneration.BestChromosome as VRPChromosome, fitness);
        };

        ga.Start();
    }

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
        }
    }

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
        routes.Add(vehicleID, formattedRoute);
    }

    public void DrawRoute(List<GameObject> route)
    {
        lr.positionCount = route.Count;

        for (int i = 0; i < route.Count; i++)
        {
            DropPoint d = route[i].GetComponent<DropPoint>();
            lr.SetPosition(i, d.Position.position);
        }

        lr.SetPosition(route.Count, route[0].GetComponent<DropPoint>().Position.position);
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

    /// <summary>
    /// ACL functions
    /// </summary>
    private void RequestConstraints() { }

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
        //will need to child the package game object to the truck 

        //loop through and remove assigned packages from packagesAtDepot list 

    }

    /// <summary>
    /// properties
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
}
