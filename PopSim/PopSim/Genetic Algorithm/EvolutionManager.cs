using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class EvolutionManager
{
    private List<GeneticAgent> population = [];
    private int genomeSize = 24;
    private int generationSize = 15;
    private int generationCap = 10;

    private List<Thread> threads = [];
    
    public void Start()
    {
        Console.WriteLine("Starting Evolution Manager!");
        
        //Initial population
        for (int i = 0; i < generationSize; i++)
            population.Add(new GeneticAgent(GenerateGenome(), 0, i));
        
        
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
            
            UniformCrossover(bestAgent, secondBestAgent, out Dictionary<int, bool[]> genomeA);
            UniformCrossover(bestAgent, secondBestAgent, out Dictionary<int, bool[]> genomeB);
            
            //Clear population, and create a new one
            population.Clear();
            
            //Elitism - Best agent survives, unchanged
            population.Add(new GeneticAgent(bestAgent.genome, bestAgent.generation, 0));

            //Wildcards - 2 totally new random agents
            population.Add(new GeneticAgent(GenerateGenome(), generation + 1, 0));
            population.Add(new GeneticAgent(GenerateGenome(), generation + 1, 0));
            
            //Offspring - Based on the 2 best agents
            for (int i = population.Count; i < generationSize; i++)
            {
                Dictionary<int, bool[]> newGenome = i % 2 == 0 ? genomeA : genomeB;
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
    
    private Dictionary<int, bool[]> GenerateGenome(){
        Dictionary<int, bool[]> genome = new Dictionary<int, bool[]>();
        while (genome.Count < genomeSize)
        {
            GenerateDay(out int day,out bool[] policies);
            if (genome.ContainsKey(day))
                continue;
            
            genome.Add(day,policies);
        }

        return genome;
    }

    private void GenerateDay(out int day, out bool[] policiesList)
    {
            day = RandomManager.Instance.GetNextInt(AlgorithmParameters.Instance.simDuration/24);
        
            policiesList = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                policiesList[i] = RandomManager.Instance.GetNextInt(2) == 0;
            } 
    }

    private Dictionary<int, bool[]> UniformCrossover(GeneticAgent parentAgentA, GeneticAgent parentAgentB, out Dictionary<int, bool[]> genome)
    {
        genome = new Dictionary<int, bool[]>();

        List<int> dayA = new List<int>(parentAgentA.genome.Keys);
        List<int> dayB = new List<int>(parentAgentB.genome.Keys);
        int length = dayA.Count > dayB.Count ? dayB.Count : dayA.Count;
        
        for (int i = 0; i < length; i++)
        {
            if (RandomManager.Instance.GetNextInt(2) == 1)
            {
                genome.TryAdd(dayA[i], parentAgentA.genome[dayA[i]]);
            }
            else 
            { 
                genome.TryAdd(dayB[i], parentAgentB.genome[dayB[i]]);
            }
        }
        return genome;
    }

    private Dictionary<int, bool[]> Mutate(Dictionary<int,bool[]> genome, int maxMutations = 2, double mutationRate = 0.5)
    {
        List<int> days = new List<int>(genome.Keys);
        
            for (int i = 0; i < maxMutations; i++)
            {
                int r = RandomManager.Instance.GetNextInt(days.Count);
                if (RandomManager.Instance.GetNextDouble() <= mutationRate)
                {
                    genome.Remove(days[r]);
                    GenerateDay(out int day, out bool[] policies);
                    genome.TryAdd(day, policies);
                }
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

    private void Domination(List<GeneticAgent> geneticAgents)
    {
        foreach (GeneticAgent agent in geneticAgents)
        {
            foreach (GeneticAgent otherAgent in geneticAgents)
            {
                if (agent.happinessAverage >= otherAgent.happinessAverage &&
                    agent.deathRateAverage <= otherAgent.deathRateAverage &&
                    agent.happinessAverage > otherAgent.happinessAverage ||
                    agent.deathRateAverage < otherAgent.deathRateAverage)
                {
                    otherAgent.dominationCount++;
                    agent.dominates.Add(otherAgent.agentId);
                }
                    
            }
            if (agent.dominationCount == 0)
                agent.frontRank = 1;
        }

        foreach (GeneticAgent agent in geneticAgents)
        {
            if (agent.frontRank != 0)
            {
                foreach (GeneticAgent otherAgent in geneticAgents)
                {
                    if (otherAgent.frontRank == 0 && otherAgent.dominates.Contains(agent.agentId))
                        agent.dominationCount--;
                }

                if (agent.frontRank == 0)
                    agent.frontRank = 1;
            }
        }
    }
    
    
    
}