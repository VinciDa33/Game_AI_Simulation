using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public abstract class Agent
{
    //Identifiers for this agent
    public int generation { get; private set; }
    public int agentId { get; private set; }
    
    //The simulation world this agent will work on
    public SimWorld world;
    public bool extensiveLogging;

    public Agent(int generation, int id, bool extensiveLogging = false)
    {
        this.generation = generation;
        this.agentId = id;
        this.extensiveLogging = extensiveLogging;
    }

    public void Run()
    {
        Console.WriteLine($"> Thread G{generation}A{agentId} starting");
        
        //Initialize a new simulation world
        world = new SimWorld(extensiveLogging);
        world.InitWorld();

        //Run simulation
        for (int i = 0; i < AlgorithmParameters.Instance.simDuration; i++)
        {
            world.Step(i);
            
            //Only enact policies once per day!
            if (i % 24 == 0)
                EnactPolicies(i);
        }
                
        Console.WriteLine($"> Thread G{generation}A{agentId} finished");
    }

    public int GetFitnessDeathCount()
    {
        //Return the last value in the cumulative death count array. This value should be minimized
        return world.worldStatistics.cumulativeDeathCount[^1];
    }
    
    public long GetFitnessHappiness()
    {
        //Return the average happiness over the course of the simulation. Floored to nearest integer
        try
        {
            return (long)Math.Floor(world.worldStatistics.happinessOverTime.Sum() /
                                    (double)world.worldStatistics.happinessOverTime.Length);
        }
        catch (OverflowException oe)
        {
            Console.WriteLine("An overflow exception was caught");
            return int.MinValue;
        }
    }

    protected abstract void EnactPolicies(int timeStep);
    public abstract Agent[] Crossover(Agent mate, int newGeneration, int newAgentId);
    public abstract void Mutate(int maxMutations = 2, double mutationChance = 0.5d);
}