using GeneticSharp.Domain.Chromosomes;
using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Randomizations;
using UnityEngine;

public class VRPChromosome : ChromosomeBase
{
    private readonly int numberOfDropPoints;
    private readonly int numberOfVehicles;

    public VRPChromosome(int numberOfDP, int numberOfVehicles) : base(numberOfDP)
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
        return new VRPChromosome(numberOfDropPoints);
    }

    public override IChromosome Clone()
    {
        var clone = base.Clone() as VRPChromosome;
        clone.Distance = Distance;

        return clone;
    }
}
