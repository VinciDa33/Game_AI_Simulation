using Microsoft.VisualBasic;
using PopSim.Genetic_Algorithm.Iteration_1;
using PopSim.Genetic_Algorithm.Iteration_2;
using PopSim.Genetic_Algorithm.Iteration_3;
using PopSim.Genetic_Algorithm.NSGA2;
using PopSim.Utility;

namespace PopSim.Genetic_Algorithm;

public class EvolutionManager
{
    private List<Agent> population = [];

    private int generation;
    private int generationSize;
    private int generationCount;

    public EvolutionManager(int generation = 3, int generationSize = 50, int generationCount = 50)
    {
        this.generation = generation;
        this.generationSize = generationSize;
        this.generationCount = generationCount;
        
        Console.WriteLine("Settings:");
        Console.WriteLine("- Generation " + generation);
        Console.WriteLine("- Generation size " + generationSize);
        Console.WriteLine("- Generation count " + generationCount);
    }
    
    public void Start()
    {
        LogManager logger = new LogManager();
        List<Agent> bestAgents = [];
        Console.WriteLine("- Starting Evolution Manager!");
        
        //1. Initial population
        Console.WriteLine("- Generating Population");
        GenerateInitialPopulation();
        
        //Simulate using the initial population
        Console.WriteLine("- Simulating initial generation");
        RunSimulations(population);

        for (int i = 0; i < generationCount; i++)
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
            
            Console.WriteLine("-- Generation " + i + " finished --");
            
            //Log the entire best front
            logger.dataToLog.Add($"\n<><><> GENERATION {i} <><><>");
            foreach (Agent agent in newFronts.fronts[0])
                logger.dataToLog.Add(agent.ToString());

            //In the last generation, get the best agents for extensive logging
            if (i == generationCount - 1)
            {
                if (newFronts.fronts[0].Count <= 3)
                    bestAgents.AddRange(newFronts.fronts[0]);
                else
                {
                    bestAgents.Add(newFronts.fronts[0][0]); //Get the best agent for one of the parameters
                    bestAgents.Add(newFronts.fronts[0][^1]); //Get the best agent for the other parameter
                    bestAgents.Add(newFronts.fronts[0][newFronts.fronts[0].Count / 2]); //Get a middle solution
                }
            }
        }
        
        //Log some of all generations
        logger.Log("GenerationLog");
        
        Console.WriteLine("Running extensive logging!");
        foreach (Agent agent in bestAgents)
            agent.extensiveLogging = true;
        
        RunSimulations(bestAgents);

        LogManager dataLogger = new LogManager();
        foreach (Agent agent in bestAgents)
        {
            dataLogger.dataToLog.Add("\nG" + agent.generation + "I" + agent.agentId);
            dataLogger.dataToLog.Add(agent.world.worldStatistics.ToString());
            dataLogger.dataToLog.Add(agent.world.policyManager.ToString());
        }
        dataLogger.Log("DataLog");
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
        population.Clear();
        for (int i = 0; i < generationSize; i++)
        {
            if (generation == 1)
                population.Add(new Iteration1Agent(0, i));
            else if (generation == 2)
                population.Add(new Iteration2Agent(0, i));
            else if (generation == 3)
                population.Add(new Iteration3Agent(0, i));
        }
    }

    private List<Agent> GenerateOffspring(List<Agent> parents, int newGeneration)
    {
        Console.WriteLine("- Generating offspring\n");
        
        List<Agent> offspringPool = [];

        while (offspringPool.Count < generationSize)
        {
            //Pick two random parents
            Agent parentA = parents[RandomManager.Instance.GetNextInt(parents.Count)];
            Agent parentB = parents[RandomManager.Instance.GetNextInt(parents.Count)];
            
            //Generate new offspring using crossover
            Agent[] offspring = parentA.Crossover(parentB, newGeneration, offspringPool.Count);

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
            matingPool.Add(NSGA2.NSGAII.TournamentSelection(agents, paretoFronts, crowdingDistances));

        return matingPool;
    }
}