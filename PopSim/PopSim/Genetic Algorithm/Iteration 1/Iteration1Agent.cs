using PopSim.Policies;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm.Iteration_1;

public class Iteration1Agent : Agent
{
    //Genome of single policy states
    public List<bool> genome = new List<bool>();
    
    public Iteration1Agent(int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
        GenerateGenome();
    }
    
    public Iteration1Agent(List<bool> genome, int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
        this.genome = genome;
    }


    private void GenerateGenome()
    {
        genome.Clear();
        int genomeSize = 8; //Currently hardcoded genome size
        for (int i = 0; i < genomeSize; i++)
            genome.Add(RandomManager.Instance.GetNextInt(0, 2) == 0); //Set random 50/50 values for genome
    }
    
    
    
    protected override void EnactPolicies(int timeStep)
    {
        //Follow the genome to enact policies
        for (int i = 0; i < world.policyManager.policies.Length; i++)
        {
            if (genome[i])
                world.policyManager.policies[i].EnablePolicy(timeStep);
            else
                world.policyManager.policies[i].DisablePolicy(timeStep);
        }
    }

    public override Agent[] Crossover(Agent mate, int newGeneration, int newAgentId)
    {
        //If the other agent is not of iteration 1, someone has made a mistake
        if (mate is not Iteration1Agent)
        {
            Console.WriteLine("You have made a mistake...");
            return [new Iteration1Agent(-1, -1)];
        }
        
        //Cast the other agent
        Iteration1Agent mateIt1 = (Iteration1Agent) mate;

        List<bool> newGenome = [];
        
        //Pick random values from parent genomes
        for (int i = 0; i < genome.Count; i++)
        {
            if (RandomManager.Instance.GetNextInt(2) == 0)
                newGenome.Add(genome[i]);
            else
                newGenome.Add(mateIt1.genome[i]);
        }

        return [new Iteration1Agent(newGenome, newGeneration, newAgentId)];
    }

    public override void Mutate(int maxMutations = 2, double mutationChance = 0.5)
    {
        //Do up to 2 mutations
        for (int i = 0; i < maxMutations; i++)
        {
            //Get a random index
            int randomIndex = RandomManager.Instance.GetNextInt(genome.Count);
            
            //If the mutation chance is hit, flip the index value
            if (RandomManager.Instance.GetNextDouble() < mutationChance)
                genome[randomIndex] = !genome[randomIndex];
        }
    }
    
    public override string ToString()
    {
        string str = $"Agent = G{generation}A{agentId}\n";
        str += $"Death Fitness {GetFitnessDeathCount()}\n";
        str += $"Happiness Fitness {GetFitnessHappiness()}\n";

        str += "Genome = [";
        str += string.Join(", ", genome);
        str += "]\n\n";

        return str;
    }
}