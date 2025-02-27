﻿using System.Buffers;
using GenAlg.Common;

namespace GenAlg.PushThePoint;

public class Genome : IGenome<Genome>
{
    public GenomeAction[] ActionsSequence;
    const int GenesLength = 200;
    public int Length => GenesLength;

    public double Fitness;
    public bool FitnessCalculated = false;
    
    public Genome()
    {
        ActionsSequence = Program.ProgramArrayPool.Rent(GenesLength);
        for (var i = 0; i < GenesLength; i++)
        {
            ActionsSequence[i] = (GenomeAction)Program.Random.Next(5);
        }
    }

    public Genome(GenomeAction[] actions, double fitness, bool fitnessCalculated)
    {
        ActionsSequence = Program.ProgramArrayPool.Rent(GenesLength);
        for (var i = 0; i < GenesLength; i++)
        {
            ActionsSequence[i] = actions[i];
        }

        Fitness = fitness;
        FitnessCalculated = fitnessCalculated;
    }

    public Genome Clone()
    {
        return new Genome(ActionsSequence, Fitness, FitnessCalculated);
    }

    public (Genome, Genome) Cross(Genome other)
    {
        FitnessCalculated = false;
        other.FitnessCalculated = false;
        var s = Program.Random.Next(2, GenesLength - 3);
        if (s < GenesLength - s)
            for (var i = 0; i < s; i++)
                (ActionsSequence[i], other.ActionsSequence[i]) = (other.ActionsSequence[i], ActionsSequence[i]);
        else
            for (var i = s; i < GenesLength; i++)
                (ActionsSequence[i], other.ActionsSequence[i]) = (other.ActionsSequence[i], ActionsSequence[i]);

        return (this, other);
    }

    public Genome Mutate()
    {
        FitnessCalculated = false;
        for (var i = 0; i < 5; i++)
        {
            var index = Program.Random.Next(0, GenesLength);
            ActionsSequence[index] = (GenomeAction)Program.Random.Next(5);
        }

        return this;
    }

    public void Dispose()
    {
        Program.ProgramArrayPool.Return(ActionsSequence);
    }

    public void SetFitness(double fitness)
    {
        Fitness = fitness;
        FitnessCalculated = true;
    }
}