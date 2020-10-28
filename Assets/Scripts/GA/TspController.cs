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

/// <summary>
/// heavily adapted from https://diegogiacomelli.com.br/tsp-with-GeneticSharp-and-Unity3d/
/// TSP GA runner 
/// </summary>
public class TspController : MonoBehaviour
{
    private GeneticAlgorithm ga;


    /// <summary>
    /// returns the list of drop points in order - the route
    /// </summary>
    /// <param name="dropPoints"></param>
    /// <returns></returns>
    public List<GameObject> CalculateGA(List<GameObject> dps, int numberOfGenerations)
    {
        List<GameObject> route = new List<GameObject>();
        var fitness = new TspFitness(dps);
        var chromosome = new TspChromosome(dps.Count);

        var crossover = new OrderedCrossover();
        var mutation = new ReverseSequenceMutation();
        var selection = new RouletteWheelSelection();
        var population = new Population(50, 100, chromosome);

        ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
        ga.Termination = new GenerationNumberTermination(numberOfGenerations);

        ga.GenerationRan += delegate
        {
            var distance = ((TspChromosome)ga.BestChromosome).Distance;
            Debug.Log($"Generation: {ga.GenerationsNumber} - Distance: ${distance}  - Fitness: ${ga.BestChromosome.Fitness}");
        };
        ga.Start();


        var c = ga.Population.CurrentGeneration.BestChromosome as TspChromosome;

        if (c != null)
        {
            var genes = c.GetGenes();
            var dropPoints = ((TspFitness)ga.Fitness).DropPoints;
            for (int i = 0; i < genes.Length; i++) 
            {
                route.Add(dropPoints[(int)genes[i].Value].gameObject);
            }
        }
        return route; 
    }
}
