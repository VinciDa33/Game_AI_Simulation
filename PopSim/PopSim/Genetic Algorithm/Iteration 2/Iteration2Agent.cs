using PopSim.Utility;

namespace PopSim.Genetic_Algorithm.Iteration_2;

public class Iteration2Agent : Agent
{
    //A set of chance modifiers at different time steps for enabling or disabling policies
    public float[] genome;
    
    public Iteration2Agent(int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
        GenerateGenome();
    }
    
    public Iteration2Agent(float[] genome, int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
        this.genome = genome;
    }

    private void GenerateGenome()
    {
        genome = new float[24]; //Hardcoded size based on '3 x number of policies'
        for (int i = 0; i < genome.Length; i++)
            genome[i] = (float) RandomManager.Instance.GetNextDouble(-0.6d, 0.6d);
    }
    
    protected override void EnactPolicies(int timeStep)
    {
        for (int i = 0; i < world.policyManager.policies.Length; i++)
        {
            int geneIndex = i * 3; //Each policy has 3 genes, 1 for each time area (start, mid, end)
            float progress = timeStep / (float) AlgorithmParameters.Instance.simDuration; //Get how far through the simulation we currently are. In range 0-1
            
            //Offset index based on progress through the sim
            if (progress > 0.66f) //Go to the 'end of simulation' genes
                geneIndex += 2;
            else if (progress > 0.33f) //Go to the 'mid of simulation' genes
                geneIndex += 1;
            
            //Get a random value from -0.5 to 0.5
            double rand = RandomManager.Instance.GetNextDouble(-0.5d, 0.5d);
            
            //Offset the random value using this agents genome (this is how the genome get to act on decisions)
            rand += genome[geneIndex];
            
            //If the value is large, enable this policy. If it is low, disable it
            if (rand > 0.75d)
                world.policyManager.policies[i].EnablePolicy(timeStep);
            else if (rand < -0.75d)
                world.policyManager.policies[i].DisablePolicy(timeStep);
        }
    }

    public override Agent[] Crossover(Agent mate, int newGeneration, int newAgentId)
    {
        //If the other agent is not of iteration 1, someone has made a mistake
        if (mate is not Iteration2Agent)
        {
            Console.WriteLine("You have made a mistake...");
            return [new Iteration2Agent(-1, -1)];
        }
        
        //Cast the other agent
        Iteration2Agent mateIt2 = (Iteration2Agent) mate;
        
        
        int size = genome.Length;
        
        float[] offspringAGenome = new float[size];
        float[] offspringBGenome = new float[size];
        
        int p = RandomManager.Instance.GetNextInt(1, size);

        for (int i = 0; i < size; i++)
        {
            if (i <= p)
            {
                offspringAGenome[i] = this.genome[i];
                offspringBGenome[i] = mateIt2.genome[i];
            }
            else
            {
                offspringAGenome[i] = mateIt2.genome[i];
                offspringBGenome[i] = this.genome[i];
            }
        }

        return [new Iteration2Agent(offspringAGenome, newGeneration, newAgentId), new Iteration2Agent(offspringAGenome, newGeneration, newAgentId + 1)];
    }

    public override void Mutate(int maxMutations = 2, double mutationChance = 0.5)
    {
        //get an amount of mutations to do
        int numberOfMutations = RandomManager.Instance.GetNextInt(maxMutations + 1);
        
        for (int i = 0; i < numberOfMutations; i++)
        {
            //Change a random index by a random amount
            int randomIndex = RandomManager.Instance.GetNextInt(genome.Length);
            genome[randomIndex] += (float) RandomManager.Instance.GetNextDouble(-0.3d, 0.3d);
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