using Microsoft.VisualBasic.CompilerServices;
using PopSim.Sim;
using PopSim.Utility;

namespace PopSim.Genetic_Algorithm;

public class AlgorithmManager
{
    private List<Algorithm> population = new List<Algorithm>();
    private int generationSize = 10;
    private int generationCap = 100;

    private List<Thread> threads = new List<Thread>();
    
    public void Start()
    {
        Console.WriteLine("Starting Algorithm Manager!");
        GeneratePopulation(generationSize, 0);
        
        for (int generation = 0; generation < generationCap; generation++)
        {
            Console.WriteLine($"Spinning up generation {generation}");
            
            threads.Clear();
            for (int i = 0; i < population.Count; i++)
            {
                Thread t = new Thread(new ThreadStart(population[i].Start));
                t.Start();
                threads.Add(t);
                //population[i].Start();
            }

            while (ThreadsRunning())
            {
                //The show must go on
            }

            Console.WriteLine("-- Generation Finished --");

            Algorithm[] bestAlgorithms = Selection();
            if (generation % 1 == 0)
                LogManager.Instance.dataToLog.Add(bestAlgorithms[0].ToString());
            
            List<bool>[] offspringGenomes = SinglePointCrossover(bestAlgorithms);
            List<List<bool>> newGenomes = new List<List<bool>>();
            
            for (int i = 0; i < generationSize; i++)
                newGenomes.Add(offspringGenomes[i%2]);
        
            for (int i = 2; i < generationSize; i++)
                Mutation(newGenomes[i]);

            population.Clear();
            for (int i = 0; i < generationSize; i++)
                population.Add(new Algorithm(newGenomes[i], generation + 1));
        }
        
        LogManager.Instance.Log();
    }

    private bool ThreadsRunning()
    {
        foreach (Thread t in threads)
            if (t.IsAlive)
                return true;

        return false;
    }
    
    private List<bool> GenerateGenome()
    {
        List<bool> genome = new List<bool>();
        int genomeSize = SimParameters.Instance.policiesList.Count;
        for (int i = 0; i < genomeSize; i++)
        {
            genome.Add(RandomManager.Instance.GetNextInt(0, 2) == 0);
        }
        return genome;
    }
    
    private void GeneratePopulation(int size, int generation)
    {
        for (int i = 0; i < size; i++)
            population.Add(new Algorithm(GenerateGenome(), generation));
    }

    private List<bool>[] SinglePointCrossover(Algorithm[] parents)
    {
        List<bool> genomeA = parents[0].genome; 
        List<bool> genomeB = parents[1].genome;
        
        int p = RandomManager.Instance.GetNextInt(1, genomeA.Count);
        
        List<bool> offspringA = new List<bool>();
        offspringA.AddRange(genomeA.GetRange(0,p));
        offspringA.AddRange(genomeB.GetRange(p, genomeA.Count - p));

        List<bool> offspringB = new List<bool>();
        offspringB.AddRange(genomeB.GetRange(0,p));
        offspringB.AddRange(genomeA.GetRange(p, genomeB.Count - p));
        
        return [offspringA, offspringB];
    }

    private void Mutation(List<bool> genome, int maxMutations = 2, double probability = 0.5)
    {
        for (int i = 0; i < maxMutations; i++)
        {
            int r = RandomManager.Instance.GetNextInt(genome.Count);
            if (RandomManager.Instance.GetNextDouble() <= probability)
                genome[r] = !genome[r];
        }
    }

    private Algorithm[] Selection()
    {
        float[] selection = [float.MinValue, float.MinValue];
        int[] index = new int[2];
        
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].FitnessValue() > selection[0])
            {
                selection[1] = selection[0];
                selection[0] = population[i].FitnessValue();
                index[1] = index[0];
                index[0] = i;
            }
            else if (population[i].FitnessValue() > selection[1])
            {
                selection[1] = population[i].FitnessValue();
                index[1] = i;
            }
        }

        return [population[index[0]], population[index[1]]];
    }
}