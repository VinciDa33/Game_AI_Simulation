using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class EvolutionManager
{
    private List<GeneticAgent> population = [];
    private List<GeneticAgent> matingPool = new List<GeneticAgent>();
    private List<GeneticAgent> offspringPool = new List<GeneticAgent>();
    private int genomeSize = 24;
    private int generationSize = 15;
    private int generationCap = 10;

    private List<Thread> threads = [];
    
    public void Start()
    {
        Console.WriteLine("Starting Evolution Manager!");
        
        //Initial population
        GeneratePopulation();
        
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

        
        //Run multiple generations
        for (int generation = 1; generation < generationCap; generation++)
        {
            if (generation != 1)
                population.AddRange(offspringPool);
            
            Domination(population);
            Console.WriteLine("Crowding");
            crowding(population);
            
            if (generation != 1)
            {
                Console.WriteLine("Truncating");
                DeterministicTruncation(population);
            }
            Console.WriteLine("Generating mating pool");
            GenerateMatingPool();
            Console.WriteLine("Generating offspring pool");
            GenerateOffspring(matingPool, generation);
            
            Console.WriteLine($"Spinning up generation {generation}");
            //Create a thread for each agent, and start their simulation
            threads.Clear();
            for (int i = 0; i < offspringPool.Count; i++)
            {
                Thread t = new Thread(offspringPool[i].Start);
                t.Start();
                threads.Add(t);
            }

            while (ThreadsRunning())
            {
                //The show must go on
            }


            Console.WriteLine("-- Generation Finished --");
            
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

    private void GeneratePopulation()
    {
        for (int i = 0; i < generationSize; i++)
            population.Add(new GeneticAgent(GenerateGenome(), 0, i));
    }

    private void GenerateOffspring(List<GeneticAgent> parents, int generation)
    {
        offspringPool.Clear();
        for (int i = 0; i < generationSize; i++)
        {
            GeneticAgent parentA = parents[RandomManager.Instance.GetNextInt(parents.Count)];
            GeneticAgent parentB = parents[RandomManager.Instance.GetNextInt(parents.Count)];
            offspringPool.Add(new GeneticAgent(Mutate(UniformCrossover(parentA,parentB)), generation, i));
        }
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

    private Dictionary<int, bool[]> UniformCrossover(GeneticAgent parentAgentA, GeneticAgent parentAgentB)
    {
        Dictionary<int, bool[]> genome = new Dictionary<int, bool[]>();

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

    private GeneticAgent Selection(List<GeneticAgent> agents)
    {
        GeneticAgent parentA = agents[RandomManager.Instance.GetNextInt(agents.Count)];
        GeneticAgent parentB = agents[RandomManager.Instance.GetNextInt(agents.Count)];

        if (parentA.frontRank<parentB.frontRank)
            return parentA;
        if (parentB.frontRank<parentA.frontRank)
            return parentB;
        if (parentA.crowdingDistance>=parentB.crowdingDistance)
        {
            return parentA;
        }
        return parentB;
    }

    private void GenerateMatingPool()
    {
        matingPool.Clear();
        for (int i = 0; i < generationSize; i++)
        {
            matingPool.Add(Selection(population));
        }
    }

    private void Domination(List<GeneticAgent> geneticAgents)
    {
        foreach (GeneticAgent g in geneticAgents)
        {
            g.frontRank = 0;
            g.dominates.Clear();
        }
        foreach (GeneticAgent agent in geneticAgents)
        {
            foreach (GeneticAgent otherAgent in geneticAgents)
            {
                if (agent.happinessAverage >= otherAgent.happinessAverage &&
                    agent.deathRateAverage <= otherAgent.deathRateAverage &&
                    (agent.happinessAverage > otherAgent.happinessAverage ||
                    agent.deathRateAverage < otherAgent.deathRateAverage))
                {
                    otherAgent.dominationCount++;
                    agent.dominates.Add(otherAgent.agentId);
                }
                    
            }
        }
        
        foreach (GeneticAgent agent in geneticAgents)
        {
            if (agent.dominationCount == 0)
                agent.frontRank = 1;
        }

        int index = 1;
        while (MoreRanks(geneticAgents))
        {
            Console.WriteLine("Trying to assign ranks" + index);
            RankAssignment(index ,geneticAgents);
            index++;
        }
    }

    //CR = Current rank, WR = Without rank
    private void RankAssignment(int geneticAgentsCR, List<GeneticAgent> geneticAgents)
    {
        foreach (GeneticAgent agent in geneticAgents)
        {
            if (agent.frontRank <= geneticAgentsCR && agent.frontRank != 0)
                continue;
            foreach (GeneticAgent otherAgent in geneticAgents) 
            { 
                if (otherAgent.frontRank == geneticAgentsCR && otherAgent.dominates.Contains(agent.agentId)) 
                    agent.dominationCount--;
            }
            if (agent.dominationCount == 0) 
                agent.frontRank = geneticAgentsCR+1;
        }
    }

    private bool MoreRanks(List<GeneticAgent> geneticAgents)
    {
        foreach (GeneticAgent agent in geneticAgents)
            if (agent.frontRank == 0)
                return true;
        return false;
    }

    private void crowding(List<GeneticAgent> geneticAgents)
    {
        geneticAgents.Sort(new DeathComparator());
        List<GeneticAgent> rank1 = new List<GeneticAgent>();
        foreach (GeneticAgent agent in geneticAgents)
        {
            if (agent.frontRank == 1)
            {
                rank1.Add(agent);
            }
        }

        for (int i = 0; i < rank1.Count; i++)
        {
            if (i==0)
                rank1[i].crowdingDistance = double.MaxValue;
            else if(i == rank1.Count - 1)
                rank1[i].crowdingDistance = double.MaxValue;
            else
                rank1[i].crowdingDistance += (rank1[i + 1].cumulativeDeathCount[^1]-rank1[i - 1].cumulativeDeathCount[^1])/(rank1[^1].cumulativeDeathCount[^1]-rank1[0].cumulativeDeathCount[^1]);
        }
        
        
        geneticAgents.Sort(new HappinessComparator());
        rank1.Clear();
        foreach (GeneticAgent agent in geneticAgents)
        {
            if (agent.frontRank == 1)
            {
                rank1.Add(agent);
            }
        }

        for (int i = 0; i < rank1.Count; i++)
        {
            if (i==0)
                rank1[i].crowdingDistance = double.MaxValue;
            else if(i == rank1.Count - 1)
                rank1[i].crowdingDistance = double.MaxValue;
            else
                rank1[i].crowdingDistance += (rank1[i + 1].happinessAverage-rank1[i - 1].happinessAverage)/(rank1[^1].happinessAverage-rank1[0].happinessAverage);
        }
    }

    private void DeterministicTruncation(List<GeneticAgent> geneticAgents)
    {
        List<GeneticAgent> newPop = new List<GeneticAgent>();
        int rank = 1;
        while (newPop.Count < generationSize)
        {
            int agentsOfCurrentRank = 0;
            for (int i = 0; i < geneticAgents.Count; i++)
            {
                if (geneticAgents[i].frontRank == rank)
                {
                    agentsOfCurrentRank++;
                }
            }
            if (agentsOfCurrentRank + newPop.Count <= generationSize)
            {
                foreach (GeneticAgent agent in geneticAgents)
                {
                    if (agent.frontRank == rank)
                    {
                        newPop.Add(agent);
                    }
                }
            }
            else
            {
                geneticAgents.Sort(new CrowdingComparator());
                foreach (GeneticAgent agent in geneticAgents)
                {
                    if (agent.frontRank == rank)
                        newPop.Add(agent);
                    if (newPop.Count == generationSize)
                        break;
                }
            }
            rank++;
        }
        geneticAgents.Clear();
        geneticAgents.AddRange(newPop);
    }
    
}