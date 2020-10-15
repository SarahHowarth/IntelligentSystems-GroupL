using System;        
using System.Collections.Generic;       
using System.Globalization;        
using System.Linq;                    
using GeneticSharp.Domain.Chromosomes;                    
using GeneticSharp.Domain.Fitnesses;           
using GeneticSharp.Domain.Randomizations;
using UnityEngine;

public class TspFitness : IFitness
{
    private Rect area;

    public TspFitness(int numberOfDropPoints) 
    {
        var dropPoints = new List<DropPoint>(numberOfDropPoints);
        var size = Camera.main.orthographicSize - 1;
        area = new Rect(-size, -size, size * 2, size * 2);

        for (int i = 0; i < numberOfDropPoints; i++) 
        { 
            //var 
        }
    }
}
