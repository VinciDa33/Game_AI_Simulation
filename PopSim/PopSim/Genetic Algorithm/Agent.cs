using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public abstract class Agent
{
    //Identifiers for this agent
    public int generation { get; private set; }
    public int agentId { get; private set; }
    
    //The simulation world this agent will work on
    protected SimWorld world;
    

    public Agent(int generation, int id)
    {
        this.generation = generation;
        this.agentId = id;
    }

    public void Run()
    {
        Console.WriteLine($"> Thread G{generation}A{agentId} starting");
        
        //Initialize a new simulation world
        world = new SimWorld();
        world.InitWorld();

        //Run simulation
        for (int i = 0; i < AlgorithmParameters.Instance.simDuration; i++)
        {
            world.Step(i);
            EnactPolicies(i);
        }
                
        Console.WriteLine($"> Thread G{generation}A{agentId} finished");
    }

    public int GetFitnessDeathCount()
    {
        //Return the last value in the cumulative death count array. This value should be minimized
        return world.worldStatistics.cumulativeDeathCount[^1];
    }
    
    public int GetFitnessHappiness()
    {
        //Return the average happiness over the course of the simulation. Floored to nearest integer
        return (int) Math.Floor(world.worldStatistics.happinessOverTime.Sum() / (float)world.worldStatistics.happinessOverTime.Length);
    }

    protected abstract void EnactPolicies(int timeStep);
    public abstract Agent Crossover(Agent mate, int generation, int newAgentId);
    public abstract void Mutate(int maxMutations = 2, double mutationChance = 0.5d);
}