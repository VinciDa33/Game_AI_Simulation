using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

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
    private float deathRateAverage {get; set;}
    private float happinessAverage {get; set;}
    
    
    public GeneticAgent(Dictionary<int, bool[]> genome, int generation, int agentId)
    {
        this.genome = genome;
        this.generation = generation;
        this.agentId = agentId;
        world = new SimWorld();
    }
    
    public void Start()
    {
        world = new SimWorld();
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
        //infectionValues.Add(world.worldStats.infectedCount);
        world.worldStats.UpdateStats();
        
        if (cumulativeDeathCount.Count != 0)
            deathRateValues.Add(world.worldStats.deathCount - cumulativeDeathCount[^1]);
        else
            deathRateValues.Add(world.worldStats.deathCount);
        
        cumulativeDeathCount.Add(world.worldStats.deathCount);
        happinessValues.Add(world.worldStats.happiness);
    }

    public float FitnessValue()
    {
        return -cumulativeDeathCount[^1];
        //return happinessAverage - deathRateAverage;
    }

    public override string ToString()
    {
        world.worldStats.UpdateStats();
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