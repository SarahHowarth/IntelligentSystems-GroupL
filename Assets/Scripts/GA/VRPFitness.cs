using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Randomizations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// VRP Fitness function
/// </summary>
public class VRPFitness : IFitness
{
    private readonly int OVERLOADED_TRUCK_PENALTY = 200; //if a truck is overloaded
    private readonly int UNUSED_TRUCK_PENALTY = 20; //if a truck has unused capacity
    private readonly int EMPTY_TRUCK_PENALTY = 80; //if a truck is empty
    private readonly int DISTANCE_PENALTY = 4;
    private Depot depot;
    
    public VRPFitness(Depot d)
    {
        depot = d;
    }

    //Fitness evaluator
    public double Evaluate(IChromosome chromosome)
    {
        int numberOfVehicles = depot.DeliveryAgents.Count();
        double totalDemand = 0.0;
        double totalDistance = 0.0;
        double totalDemandCost = 0.0;
        double fitness = 0.0;

        for (int i = 0; i < numberOfVehicles; i++) 
        {
            double vehicleCapacity = depot.GetVehicleCapacity(i);
            totalDistance = CalcTotalDistance(i, chromosome);
            totalDemand = CalcTotalDemand(i, chromosome);

            if (totalDemand == 0)
            {
                totalDemandCost = ((vehicleCapacity - totalDemand) * EMPTY_TRUCK_PENALTY);
            }
            else if (totalDemand > vehicleCapacity)
            {
                totalDemandCost = ((totalDemand - vehicleCapacity) * OVERLOADED_TRUCK_PENALTY);
            }
            else 
            {
                totalDemandCost = ((vehicleCapacity - totalDemand) * UNUSED_TRUCK_PENALTY);
            }

            fitness += (DISTANCE_PENALTY * totalDistance) - totalDemandCost;
        }
        
        if (fitness < 0)
        {
            return 0;
        }

        return fitness;
    }

    //calculate total distance for truck
    public double CalcTotalDistance(int vehicleID, IChromosome chromosome) 
    {
        double totalDistance = 0.0;
        List<int> positions = GetPositions(vehicleID, chromosome);
        Vector3 depotPosition = depot.transform.position;
        Vector3 lastVisited = depotPosition;

        foreach (int p in positions) 
        {
            GameObject dropPoint = depot.DropPoints[p];
            totalDistance += Vector3.Distance(lastVisited, dropPoint.transform.position);
            lastVisited = dropPoint.transform.position;
        }

        //return to depot
        totalDistance += Vector3.Distance(lastVisited, depotPosition);

        return totalDistance;      
    }

    //calculate total demand for truck
    public double CalcTotalDemand(int vehicleID, IChromosome chromosome) 
    {
        double totalDemand = 0.0;
        List<int> positions = GetPositions(vehicleID, chromosome);

        foreach (int p in positions)
        {
            GameObject dropPointObject = depot.DropPoints[p];
            DropPoint dp = dropPointObject.GetComponent<DropPoint>();
            totalDemand += depot.GetDropPointDemand(dp);
        }

        return totalDemand;
    }

    public List<int> GetPositions(int vehicleID, IChromosome chromosome)
    {
        List<int> positions = new List<int>();
        for (int i = 0; i < depot.DropPoints.Count(); i++) 
        {
            int value = (int)chromosome.GetGene(i).Value;
            if (value == vehicleID) 
            {
                positions.Add(i);
            }
        }
        //order route

        return positions;
    }
}
