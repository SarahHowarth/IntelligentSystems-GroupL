using System;        
using System.Collections.Generic;       
using System.Globalization;        
using System.Linq;                    
using GeneticSharp.Domain.Chromosomes;                    
using GeneticSharp.Domain.Fitnesses;           
using GeneticSharp.Domain.Randomizations;
using UnityEngine;


/// <summary>
/// heavily adapted from https://diegogiacomelli.com.br/tsp-with-GeneticSharp-and-Unity3d/
/// </summary>
public class TspFitness : IFitness
{
    public TspFitness(List<GameObject> dropPoints) 
    {
        DropPoints = new List<DropPoint>(dropPoints.Count);
        foreach(GameObject d in dropPoints) 
        {
            DropPoints.Add(d.GetComponent<DropPoint>());
        }
    }

    public IList<DropPoint> DropPoints { get; private set; }

    public double Evaluate(IChromosome chromosome) 
    {
        var genes = chromosome.GetGenes();
        var distanceSum = 0.0;
        var lastDropPointIndex = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture);
        var dropPointIndexes = new List<int>();
        dropPointIndexes.Add(lastDropPointIndex);

        //calculates the total route distance 
        foreach (var g in genes) 
        {
            var currentDropPointIndex = Convert.ToInt32(g.Value, CultureInfo.InvariantCulture);
            distanceSum += CalcDistanceTwoCities(DropPoints[dropPointIndexes.Last()], DropPoints[dropPointIndexes.First()]);
            lastDropPointIndex = currentDropPointIndex;
            dropPointIndexes.Add(lastDropPointIndex);
        }

        distanceSum += CalcDistanceTwoCities(DropPoints[dropPointIndexes.Last()], DropPoints[dropPointIndexes.First()]);

        var fitness = 1.0 - (distanceSum / (DropPoints.Count * 1000.0));

        ((TspChromosome)chromosome).Distance = distanceSum;

        //There is repeated cities on the indexes?
        var diff = DropPoints.Count() - dropPointIndexes.Distinct().Count();
        if (diff > 0) 
        {
            fitness /= diff;
        }

        if (fitness < 0) 
        {
            fitness = 0;
        }

        return fitness;
    }

    private static double CalcDistanceTwoCities(DropPoint one, DropPoint two) 
    {
        return Vector3.Distance(one.Position.position, two.Position.position);
    }
}
