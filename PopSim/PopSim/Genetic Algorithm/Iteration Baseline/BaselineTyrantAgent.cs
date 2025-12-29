using PopSim.Policies;

namespace PopSim.Genetic_Algorithm.Iteration_Baseline;

public class BaselineTyrantAgent : Agent
{
    public BaselineTyrantAgent(int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
    }

    protected override void EnactPolicies(int timeStep)
    {
        //Enable everything immediately
        foreach (var t in world.policyManager.policies)
            t.EnablePolicy(timeStep);
    }

    public override Agent[] Crossover(Agent mate, int newGeneration, int newAgentId)
    {
        return [new BaselineTyrantAgent(newGeneration, newAgentId)];
    }

    public override void Mutate(int maxMutations = 2, double mutationChance = 0.5)
    {
        //No mutation
    }
    
    public override string ToString()
    {
        string str = $"Agent = G{generation}A{agentId}\n";
        str += $"Death Fitness {GetFitnessDeathCount()}\n";
        str += $"Happiness Fitness {GetFitnessHappiness()}\n";
        
        return str;
    }
}