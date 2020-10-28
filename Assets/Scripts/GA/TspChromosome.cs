using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using UnityEngine;

/// <summary>
/// heavily adapted from https://diegogiacomelli.com.br/tsp-with-GeneticSharp-and-Unity3d/
/// TSP Chromosome for DA
/// </summary>
public class TspChromosome : ChromosomeBase
{
    private readonly int numberOfDropPoints;

    public TspChromosome(int numberOfDP) : base(numberOfDP)
    {
        numberOfDropPoints = numberOfDP;
        var dropPointsIndexes = RandomizationProvider.Current.GetUniqueInts(numberOfDropPoints, 0, numberOfDropPoints);

        for (int i = 0; i < numberOfDropPoints; i++) 
        {
            ReplaceGene(i, new Gene(dropPointsIndexes[i]));
        }
    }

    public double Distance { get; internal set; }

    public override Gene GenerateGene(int geneIndex)
    {
        return new Gene(RandomizationProvider.Current.GetInt(0, numberOfDropPoints));
    }

    public override IChromosome CreateNew()
    {
        return new TspChromosome(numberOfDropPoints);
    }

    public override IChromosome Clone()
    {
        var clone = base.Clone() as TspChromosome;
        clone.Distance = Distance;

        return clone;
    }
}
