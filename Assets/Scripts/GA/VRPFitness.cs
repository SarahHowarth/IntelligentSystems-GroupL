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
    private readonly int OVERLOADED_TRUCK_PENALTY = 15;
    private readonly int EMPTY_TRUCK_PENALTY = 5;
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
        double fitness = 0.0;
        double totalDemand = 0.0;
        
        for (int i = 0; i < numberOfVehicles; i++) 
        {
            float vehicleCapacity = depot.GetVehicleCapacity(i);
            fitness += CalcTotalDistance(i, chromosome) * DISTANCE_PENALTY ;
            totalDemand = CalcTotalDemand(i, chromosome);
            if (totalDemand == 0)
            {
                fitness += (vehicleCapacity - totalDemand) * EMPTY_TRUCK_PENALTY;
            }
            else if (totalDemand > vehicleCapacity)
            {
                fitness += Math.Pow(totalDemand - vehicleCapacity, OVERLOADED_TRUCK_PENALTY);
            }
        }

        if (fitness < 0)
        {
            return 0;
        }
        fitness = 1000000 - fitness;
        return Math.Max(1.0, fitness);
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
        float vehicleCapacity = depot.GetVehicleCapacity(vehicleID);

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

        return positions;
    }
}
