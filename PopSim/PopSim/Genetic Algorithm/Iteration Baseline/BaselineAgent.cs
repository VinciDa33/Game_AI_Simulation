namespace PopSim.Genetic_Algorithm.Iteration_Baseline;

public class BaselineAgent : Agent
{
    public BaselineAgent(int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
    }

    protected override void EnactPolicies(int timeStep)
    {
        //Baseline agent does not enact anything.
    }

    public override Agent[] Crossover(Agent mate, int newGeneration, int newAgentId)
    {
        return [new BaselineAgent(newGeneration, newAgentId)];
    }

    public override void Mutate(int maxMutations = 2, double mutationChance = 0.5)
    {
        //Baseline agent has nothing to mutate
    }
    
    public override string ToString()
    {
        string str = $"Agent = G{generation}A{agentId}\n";
        str += $"Death Fitness {GetFitnessDeathCount()}\n";
        str += $"Happiness Fitness {GetFitnessHappiness()}\n";
        
        return str;
    }
}