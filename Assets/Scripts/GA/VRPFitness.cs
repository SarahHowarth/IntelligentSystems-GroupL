using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Randomizations;
using UnityEngine;

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

    public IList<DropPoint> DropPoints { get; private set; }

    public double Evaluate(IChromosome chromosome)
    {
        int numberOfVehicles = depot.DeliveryAgents.Count();
        double fitness = 0;
        
        for (int i = 0; i < numberOfVehicles; i++) 
        {
            fitness += CalcTotalDistance(i, chromosome) / depot.DropPoints.Count * DISTANCE_PENALTY ;
            fitness += CalcTotalDemand(i, chromosome);
        }


        //var genes = chromosome.GetGenes();
        //var distanceSum = 0.0;
        //var lastDropPointIndex = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture);
        //var dropPointIndexes = new List<int>();
        //dropPointIndexes.Add(lastDropPointIndex);

        ////calculates the total route distance 
        //foreach (var g in genes)
        //{
        //    var currentDropPointIndex = Convert.ToInt32(g.Value, CultureInfo.InvariantCulture);
        //    distanceSum += CalcDistanceTwoCities(DropPoints[dropPointIndexes.Last()], DropPoints[dropPointIndexes.First()]);
        //    lastDropPointIndex = currentDropPointIndex;
        //    dropPointIndexes.Add(lastDropPointIndex);
        ////}

        //distanceSum += CalcDistanceTwoCities(DropPoints[dropPointIndexes.Last()], DropPoints[dropPointIndexes.First()]);

        //var fitness = 1.0 - (distanceSum / (DropPoints.Count * 1000.0));

        //((TspChromosome)chromosome).Distance = distanceSum;

        ////There is repeated cities on the indexes?
        //var diff = DropPoints.Count() - dropPointIndexes.Distinct().Count();
        //if (diff > 0)
        //{
        //    fitness /= diff;
        //}

        if (fitness < 0)
        {
            fitness = 0;
        }

        return fitness;
    }

    private double CalcTotalDistance(int vehicleID, IChromosome chromosome) 
    {
        double totalDistance = 0.0;
        return totalDistance;
        
    }

    private double CalcDistanceTwoCities(DropPoint one, DropPoint two)
    {
        return Vector3.Distance(one.Position.position, two.Position.position);
    }

    private double CalcTotalDemand(int vehicleID, IChromosome chromosome) 
    {
        return 0.0;
    }
}
