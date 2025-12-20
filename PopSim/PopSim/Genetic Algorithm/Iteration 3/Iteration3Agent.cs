using PopSim.Utility;

namespace PopSim.Genetic_Algorithm.Iteration_3;

public class Iteration3Agent : Agent
{
    public Dictionary<int, bool[]> genome =  new Dictionary<int, bool[]>();
    private int genomeSize = 24;
    
    public Iteration3Agent(int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
        GenerateGenome();
    }
    
    public Iteration3Agent(Dictionary<int, bool[]> genome, int generation, int id, bool extensiveLogging = false) : base(generation, id, extensiveLogging)
    {
        this.genome = genome;
    }

    private void GenerateGenome(){
        //Generate a new random genome for this It3Agent
        genome.Clear();
        while (genome.Count < genomeSize)
        {
            GenerateDay(out int day, out bool[] policies);
            genome.TryAdd(day, policies);
        }
    }
    
    private void GenerateDay(out int day, out bool[] policiesList)
    {
        //Generate a random day, with random policy changes
        day = RandomManager.Instance.GetNextInt((int) Math.Floor(AlgorithmParameters.Instance.simDuration/24f));
        
        policiesList = new bool[8];
        for (int i = 0; i < 8; i++)
            policiesList[i] = RandomManager.Instance.GetNextInt(2) == 0;
    }
    
    protected override void EnactPolicies(int timeStep)
    {
        //We know we can do a clean integer division by 24 (See where EnactPolicies is called in Agent)
        int currentDay = timeStep / 24;
        
        //If the genome does not contain a key for the current day, skip
        if (!genome.ContainsKey(currentDay)) 
            return;
        
        
        for (int i = 0; i < world.policyManager.policies.Length; i++)
        {
            //Enable or disable policies based on the genome values
            if (genome[currentDay][i])
                world.policyManager.policies[i].EnablePolicy(timeStep);
            else
                world.policyManager.policies[i].DisablePolicy(timeStep);
        }
    }

    public override Agent[] Crossover(Agent mate, int newGeneration, int newAgentId)
    {
        //If this triggers you have made a mistake
        if (mate is not Iteration3Agent)
        {
            Console.WriteLine("You have made a mistake...");
            return [new Iteration3Agent(-1, -1)];
        }

        Dictionary<int, bool[]> newGenome = new Dictionary<int, bool[]>();

        //Cast to correct agent type
        Iteration3Agent mateIt3 = (Iteration3Agent) mate;
        
        List<int> thisGenomeKeys = new List<int>(this.genome.Keys);
        List<int> mateGenomeKeys = new List<int>(mateIt3.genome.Keys);
        
        //Find the lenght of the shortest genome
        int length = thisGenomeKeys.Count > mateGenomeKeys.Count ? mateGenomeKeys.Count : thisGenomeKeys.Count;

        //Copy over random genomes from each parent
        for (int i = 0; i < length; i++)
        {
            if (RandomManager.Instance.GetNextInt(2) == 1)
                newGenome.TryAdd(thisGenomeKeys[i], this.genome[thisGenomeKeys[i]]);
            else
                newGenome.TryAdd(mateGenomeKeys[i], mateIt3.genome[mateGenomeKeys[i]]);
        }

        //Ensure genome size does not decrease!
        while (newGenome.Count < genomeSize)
        {
            GenerateDay(out int day, out bool[] policies);
            newGenome.TryAdd(day, policies);
        }

        return [new Iteration3Agent(newGenome, newGeneration, newAgentId)];
    }

    public override void Mutate(int maxMutations = 2, double mutationChance = 0.5)
    {
        List<int> days = new List<int>(genome.Keys);
        
        for (int i = 0; i < maxMutations; i++)
        {
            int dayIndex = RandomManager.Instance.GetNextInt(days.Count);
            if (RandomManager.Instance.GetNextDouble() <= mutationChance)
            {
                //Remove random day, and try to add a new one
                genome.Remove(days[dayIndex]);
                GenerateDay(out int newDay, out bool[] policies);
                genome.TryAdd(newDay, policies);
            }
        } 
    }

    public override string ToString()
    {
        string str = $"Agent = G{generation}A{agentId}\n";
        str += $"Death Fitness {GetFitnessDeathCount()}\n";
        str += $"Happiness Fitness {GetFitnessHappiness()}\n";
        str += "Genome: {";
        foreach (var (key, value) in genome)
        {
            string day = $"{key} => [";
            day += string.Join(", ", value);
            day += "], ";
            str += day;
        }

        str += "}\n\n";

        return str;
    }
}