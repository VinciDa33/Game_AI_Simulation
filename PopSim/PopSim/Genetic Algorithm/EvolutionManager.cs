using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class EvolutionManager
{
    private List<Agent> population = [];
    private int generationSize = 15;
    private int generationCap = 10;
    
    public void Start()
    {
        Console.WriteLine("- Starting Evolution Manager!");
        
        //1. Initial population
        Console.WriteLine("- Generating Population");
        GenerateInitialPopulation();
        
        //Simulate using the initial population
        Console.WriteLine("- Simulating initial generation");
        RunSimulations(population);

        for (int i = 0; i < generationCap; i++)
        {
            //2. Fast Non-Dominated Sorting into Pareto fronts
            ParetoFronts paretoFronts = NSGAII.GenerateParetoFronts(population);
        
            //3. Calculate crowding distances
            Dictionary<Agent, double> crowdingDistances = NSGAII.CrowdingAllFronts(paretoFronts);
            
            //4. Select agents to use for offspring creation using tournament selection
            List<Agent> matingPool = GenerateMatingPool(population, paretoFronts, crowdingDistances);

            //5. Generate a new set of offspring using crossover and mutations
            List<Agent> offspring = GenerateOffspring(matingPool, i + 1);

            //Simulate the offspring
            Console.WriteLine("- Simulating generation " + (i + 1));
            RunSimulations(offspring);

            //6. Combine parents and offspring into one list
            List<Agent> combined = [];
            combined.AddRange(population);
            combined.AddRange(offspring);

            //7. Fast Non-Dominated Sorting into Pareto fronts
            ParetoFronts newFronts = NSGAII.GenerateParetoFronts(combined);

            //Recalculate crowding distance for the new set of combined agents
            Dictionary<Agent, double> newCrowdingDistance = NSGAII.CrowdingAllFronts(newFronts);

            //8. Create a new population using deterministic truncation
            List<Agent> newPopulation = NSGAII.DeterministicTruncation(newFronts, newCrowdingDistance, generationSize);
            
            population = newPopulation;
            
            //Log one of the best current candidates. Since deterministic truncation adds the best front first, population[0] will be at least part of the best front
            LogManager.Instance.dataToLog.Add(population[0].ToString());
            Console.WriteLine("-- Generation " + i + " finished --");
        }
        
        LogManager.Instance.Log();
    }

    private void RunSimulations(List<Agent> agents)
    {
        List<Thread> threads = [];
        
        //Let each agent in the population run on their own thread
        foreach (Agent agent in agents)
        {
            Thread t = new Thread(agent.Run);
            t.Start();
            threads.Add(t);
        }
        
        //We stall until all the simulation threads finish running
        while (AnyThreadsRunning(threads))
        {
            //The show must go on
            Thread.Sleep(1000);
        }
    }

    //Checks whether any simulation threads are left alive
    private bool AnyThreadsRunning(List<Thread> threads)
    {
        foreach (Thread t in threads)
            if (t.IsAlive)
                return true;

        return false;
    }

    private void GenerateInitialPopulation()
    {
        for (int i = 0; i < generationSize; i++)
            population.Add(new Iteration3Agent(0, i));
    }

    private List<Agent> GenerateOffspring(List<Agent> parents, int generation)
    {
        Console.WriteLine("- Generating offspring\n");
        
        List<Agent> offspringPool = [];

        while (offspringPool.Count < generationSize)
        {
            //Pick two random parents
            Agent parentA = parents[RandomManager.Instance.GetNextInt(parents.Count)];
            Agent parentB = parents[RandomManager.Instance.GetNextInt(parents.Count)];
            
            //Generate new offspring using crossover
            Agent[] offspring = parentA.Crossover(parentB, generation, offspringPool.Count);

            foreach (Agent offspringAgent in offspring)
            {
                //Mutate the child
                offspringAgent.Mutate();
            
                //Add it to the offspring pool
                offspringPool.Add(offspringAgent);
            }
        }

        return offspringPool;
    }

    private List<Agent> GenerateMatingPool(List<Agent> agents, ParetoFronts paretoFronts, Dictionary<Agent, double> crowdingDistances)
    {
        Console.WriteLine("- Generating mating pool using tournament selection");
        
        List<Agent> matingPool = [];
        for (int i = 0; i < generationSize; i++)
            matingPool.Add(NSGAII.TournamentSelection(agents, paretoFronts, crowdingDistances));

        return matingPool;
    }
}