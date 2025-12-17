using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;


// THIS CLASS IS REDUNDANT AND COULD PROBABLY BE DELETED!

// THIS CLASS IS REDUNDANT AND COULD PROBABLY BE DELETED!

// THIS CLASS IS REDUNDANT AND COULD PROBABLY BE DELETED!

// THIS CLASS IS REDUNDANT AND COULD PROBABLY BE DELETED!

// THIS CLASS IS REDUNDANT AND COULD PROBABLY BE DELETED!



public class GeneticAgent
{
    public Dictionary<int, bool[]> genome =  new Dictionary<int, bool[]>();
    public int generation { get; private set; }
    public int agentId;
    
    private SimWorld world;
    private int simDuration = AlgorithmParameters.Instance.simDuration;
    
    //Fitness values
    public List<int> cumulativeDeathCount = new List<int>();
    public List<int> deathRateValues = new List<int>();
    public List<int> happinessValues = new List<int>();

    //Averages
    //private int infectionAverage {get; set;}
    public float deathRateAverage {get; set;}
    public float happinessAverage {get; set;}

    public int dominationCount = 0;
    public List<int> dominates = new List<int>();
    public int frontRank;
    public double crowdingDistance;
    
    public GeneticAgent(Dictionary<int, bool[]> genome, int generation, int agentId)
    {
        this.genome = genome;
        this.generation = generation;
        this.agentId = agentId;
        world = new SimWorld(false);
    }
    
    public void Start()
    {
        world = new SimWorld(false);
        world.InitWorld();

        //Run simulation
        for (int i = 0; i < simDuration; i++)
            Step(i);
        
        //infectionAverage = infectionValues.Sum()/infectionValues.Count;
        deathRateAverage = deathRateValues.Sum() / (float) deathRateValues.Count;
        happinessAverage = happinessValues.Sum() / (float)happinessValues.Count;
                
        Console.WriteLine($"Thread G{generation}A{agentId} finished");
    }
    
    private void Step(int timeStep)
    {
        world.Step(timeStep);
        EnactPolicies(timeStep);
        Fitness();
    }

    private void EnactPolicies(int timeStep)
    {
        if (timeStep%24 != 0) return;
        if (!genome.ContainsKey(timeStep / 24)) return;
        for (int i = 0; i < world.policyManager.policies.Length; i++)
        {
            if (genome[timeStep / 24][i] = true)
            {
                world.policyManager.policies[i].EnablePolicy();
            }
        }
    }
    
    public void Fitness()
    {
        //infectionValues.Add(world.worldState.infectedCount);
        world.worldState.UpdateState();
        
        if (cumulativeDeathCount.Count != 0)
            deathRateValues.Add(world.worldState.deathCount - cumulativeDeathCount[^1]);
        else
            deathRateValues.Add(world.worldState.deathCount);
        
        cumulativeDeathCount.Add(world.worldState.deathCount);
        happinessValues.Add(world.worldState.happiness);
    }

    public float FitnessValue()
    {
        return -cumulativeDeathCount[^1];
        //return happinessAverage - deathRateAverage;
    }

    public override string ToString()
    {
        world.worldState.UpdateState();
        string str = $"Agent = G{generation}A{agentId}\nFitness = {FitnessValue()}\n";
        str += $"Total deaths = {cumulativeDeathCount[^1]}\n";
        
        str += "Genome = [Day : ";
        List<int> keys = new List<int>(genome.Keys);
        for (int i = 0; i < genome.Count; i++)
        {
            str += keys[i];
            str += " - ";
            str += string.Join(", ", genome[keys[i]]);
        }
        
        str += "]\n";
        return str;
    }
}