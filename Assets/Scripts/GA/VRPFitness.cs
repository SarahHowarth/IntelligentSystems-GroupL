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
    private readonly int OVERLOADED_TRUCK_PENALTY = 300;
    private readonly int EMPTY_TRUCK_PENALTY = 300;
    private readonly int DISTANCE_PENALTY = 1000;
    private Depot depot;
    
    public VRPFitness(Depot d)
    {
        depot = d;
    }

    //Fitness evaluator
    public double Evaluate(IChromosome chromosome)
    {
        int numberOfVehicles = depot.DeliveryAgents.Count();
        double fitness = 0.0;
        
        for (int i = 0; i < numberOfVehicles; i++) 
        {
            fitness += CalcTotalDistance(i, chromosome) * DISTANCE_PENALTY ;
            fitness += CalcTotalDemand(i, chromosome);
        }

        if (fitness < 0)
        {
            fitness = 0;
        }
        Debug.Log("fitness = " + fitness.ToString());
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

    //calculate total deman for truck
    public double CalcTotalDemand(int vehicleID, IChromosome chromosome) 
    {
        float totalDemand = 0.0f;
        List<int> positions = GetPositions(vehicleID, chromosome);
        float vehicleCapacity = depot.GetVehicleCapacity(vehicleID);

        foreach (int p in positions)
        {
            GameObject dropPointObject = depot.DropPoints[p];
            DropPoint dp = dropPointObject.GetComponent<DropPoint>();
            totalDemand += depot.GetDropPointDemand(dp);
        }

        if (totalDemand == 0) 
        {
            return (vehicleCapacity - totalDemand) * EMPTY_TRUCK_PENALTY;
        }

        if (totalDemand > vehicleCapacity) 
        {
            return (totalDemand - vehicleCapacity) * OVERLOADED_TRUCK_PENALTY;
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

        return positions;
    }
}
