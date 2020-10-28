using GeneticSharp.Domain.Chromosomes;
using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Randomizations;
using UnityEngine;

public class VRPChromosome : ChromosomeBase
{
    private readonly int numberOfDropPoints; //chromosome length
    private readonly int numberOfVehicles; //gene variable

    /// <summary>
    /// intialise chromosome to create gene data
    /// </summary>
    /// <param name="dp">droppoints</param>
    /// <param name="v">deliveryvehicles</param>
    public VRPChromosome(int dropPointCount, int vehicleCount) : base(dropPointCount)
    {
        numberOfDropPoints = dropPointCount;
        numberOfVehicles = vehicleCount;

        for (int i = 0; i < numberOfDropPoints; i++)
        {
            ReplaceGene(i, GenerateGene(i));
        }

    }

    public override Gene GenerateGene(int geneIndex)
    {
        return new Gene(RandomizationProvider.Current.GetInt(0, numberOfVehicles)); //excludes max value but includes min value
    }

    public override IChromosome CreateNew()
    {
        return new VRPChromosome(numberOfDropPoints, numberOfVehicles);
    }

    public override IChromosome Clone()
    {
        var clone = base.Clone() as VRPChromosome;
        clone.Distance = Distance;

        return clone;
    }

    public double Distance { get; internal set; }
}
