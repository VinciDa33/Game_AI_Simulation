using Microsoft.VisualBasic.CompilerServices;
using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class EvolutionManager
{
    private List<GeneticAgent> population = [];
    private int generationSize = 15;
    private int generationCap = 80;

    private List<Thread> threads = [];
    
    public void Start()
    {
        Console.WriteLine("Starting Evolution Manager!");
        
        //Initial population
        for (int i = 0; i < generationSize; i++)
            population.Add(new GeneticAgent(GenerateGenome(24), 0, i));
        
        
        //Run multiple generations
        for (int generation = 0; generation < generationCap; generation++)
        {
            Console.WriteLine($"Spinning up generation {generation}");
            
            //Create a thread for each agent, and start their simulation
            threads.Clear();
            for (int i = 0; i < population.Count; i++)
            {
                Thread t = new Thread(population[i].Start);
                t.Start();
                threads.Add(t);
            }

            while (ThreadsRunning())
            {
                //The show must go on
            }

            Console.WriteLine("-- Generation Finished --");

            //Get the 2 most fit agents from this generation
            Selection(population, out GeneticAgent bestAgent, out GeneticAgent secondBestAgent);
            
            //Add the best agent of the generation to the log
            LogManager.Instance.dataToLog.Add(bestAgent.ToString());
            
            SinglePointCrossover(bestAgent, secondBestAgent, out float[] genomeA, out float[] genomeB);
            
            //Clear population, and create a new one
            population.Clear();
            
            //Elitism - Best agent survives, unchanged
            population.Add(new GeneticAgent(bestAgent.genome, bestAgent.generation, 0));

            //Wildcards - 2 totally new random agents
            population.Add(new GeneticAgent(GenerateGenome(24), generation + 1, 0));
            population.Add(new GeneticAgent(GenerateGenome(24), generation + 1, 0));
            
            //Offspring - Based on the 2 best agents
            for (int i = population.Count; i < generationSize; i++)
            {
                float[] newGenome = i % 2 == 0 ? genomeA : genomeB;
                population.Add(new GeneticAgent(Mutate(newGenome), generation + 1, 0));
            }

            //Update Id's
            for (int i = 0; i < population.Count; i++)
                population[i].agentId = i;
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
    
    private float[] GenerateGenome(int size)
    {
        float[] genome = new float[size];
        for (int i = 0; i < size; i++)
        {
            genome[i] = (float) RandomManager.Instance.GetNextDouble(-0.6d, 0.6d);
        }
        return genome;
    }

    private void SinglePointCrossover(GeneticAgent parentAgentA, GeneticAgent parentAgentB, out float[] genomeA, out float[] genomeB)
    {
        int size = parentAgentA.genome.Length;
        
        genomeA = new float[size];
        genomeB = new float[size];
        
        int p = RandomManager.Instance.GetNextInt(1, size);

        for (int i = 0; i < size; i++)
        {
            if (i <= p)
            {
                genomeA[i] = parentAgentA.genome[i];
                genomeB[i] = parentAgentB.genome[i];
            }
            else
            {
                genomeA[i] = parentAgentB.genome[i];
                genomeB[i] = parentAgentA.genome[i];
            }
        }
    }

    private float[] Mutate(float[] genome, int maxMutations = 5)
    {
        int numberOfMutations = RandomManager.Instance.GetNextInt(maxMutations + 1);
        
        for (int i = 0; i < maxMutations; i++)
        {
            int r = RandomManager.Instance.GetNextInt(genome.Length);
            genome[r] += (float) RandomManager.Instance.GetNextDouble(-0.25d, 0.25d);
        }

        return genome;
    }

    private void Selection(List<GeneticAgent> agents, out GeneticAgent bestAgent, out GeneticAgent secondBestAgent)
    {
        bestAgent = null;
        secondBestAgent = null;
        float bestFitness = float.MinValue;
        float secondBestFitness = float.MinValue;
        
        foreach (GeneticAgent agent in agents)
        {
            if (agent.FitnessValue() > bestFitness)
            {
                secondBestFitness = bestFitness;
                bestFitness = agent.FitnessValue();

                secondBestAgent = bestAgent;
                bestAgent = agent;
            }
            else if (agent.FitnessValue() > secondBestFitness)
            {
                secondBestFitness = agent.FitnessValue();
                secondBestAgent = agent;
            }
        }
    }
    
    
    
}