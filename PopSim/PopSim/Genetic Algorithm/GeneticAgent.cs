using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class GeneticAgent
{
    public float[] genome;
    public int generation { get; private set; }
    public int agentId;
    
    private SimWorld world;
    private int simDuration = 4383; //8766 ;
    
    //Fitness values
    public List<int> cumulativeDeathCount = new List<int>();
    public List<int> deathRateValues = new List<int>();
    public List<int> happinessValues = new List<int>();

    //Averages
    //private int infectionAverage {get; set;}
    private float deathRateAverage {get; set;}
    private float happinessAverage {get; set;}
    
    
    public GeneticAgent(float[] genome, int generation, int agentId)
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
        for (int i = 0; i < world.policyManager.policies.Length; i++)
        {
            int geneIndex = i * 3;
            float progress = timeStep / (float)simDuration;
            
            //Offset index based on progress through the sim
            if (progress > 0.66f)
                geneIndex += 2;
            else if (progress > 0.33f)
                geneIndex += 1;
            
            //Get a random value from -1 to 1
            double rand = RandomManager.Instance.GetNextDouble(-0.6d, 0.6d);
            
            //Offset the random value using this agents genome
            rand += genome[geneIndex];
            
            //If the value is large, enable this policy. If it is low, disable it
            if (rand > 0.75d)
                world.policyManager.policies[i].EnablePolicy();
            else if (rand < -0.75d)
                world.policyManager.policies[i].DisablePolicy();
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
        
        str += "Genome = [";
        str += string.Join(", ", genome);
        str += "]\n";
        return str;
    }
}