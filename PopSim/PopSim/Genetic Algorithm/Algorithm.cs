using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class Algorithm
{
    public List<bool> genome = new List<bool>();
    public int generation { get; private set; }
    
    private SimWorld world;
    public int timeStep = 0;
    private int simDuration = 4320;
    
    //Fitness values
    //public List<int> infectionValues = new List<int>();
    public List<int> deathValues = new List<int>();
    public List<int> happinessValues = new List<int>();

    //Averages
    //private int infectionAverage {get; set;}
    private float deathAverage {get; set;}
    private float happinessAverage {get; set;}
    
    
    public Algorithm(List<bool> genome, int generation)
    {
        this.genome = genome;
        this.generation = generation;
        world = new SimWorld();
    }
    
    public void Start()
    {
        world = new SimWorld();
        world.InitWorld();
        
        while (true)
        {
            Step();
            
            if (timeStep >= simDuration)
                break;
        }
        
        //infectionAverage = infectionValues.Sum()/infectionValues.Count;
        deathAverage = deathValues.Sum() / (float) deathValues.Count;
        happinessAverage = happinessValues.Sum() / (float)happinessValues.Count;
                
        Console.WriteLine("Thread dead");
    }
    
    public void Step()
    {
        world.Step(timeStep);
        Fitness();
        timeStep++;
    }
    

    public void Fitness()
    {
        if (genome.Count != SimParameters.Instance.policiesList.Count)
        {
            throw new Exception("Policies count mismatch");
        }

        for (int i = 0; i < SimParameters.Instance.policiesList.Count; i++)
        {
            if (!world.worldParameters.policyStates[0] && genome[0])
                world.worldParameters.infectionChancePerHour *= 0.80f;

            if (!world.worldParameters.policyStates[1] && genome[1])
                world.worldParameters.infectionChancePerHour *= 0.5f;

            if (!world.worldParameters.policyStates[2] && genome[2])
                world.worldParameters.infectionChancePerHour *= 0.5f;
            
            world.worldParameters.policyStates[i] = genome[i];
        }
        
        //infectionValues.Add(world.worldStats.infectedCount);
        world.worldStats.UpdateStats();
        deathValues.Add(world.worldStats.deathCount);
        happinessValues.Add(world.worldStats.happiness);
    }

    public float FitnessValue()
    {
        world.worldStats.UpdateStats();
        return -world.worldStats.deathCount;
        //return -deathAverage;
        //return happinessAverage - deathAverage;
    }

    public override string ToString()
    {
        world.worldStats.UpdateStats();
        string str = $"Generation = {generation}\nFitness = {FitnessValue()}\n";
        str += $"Total deaths = {deathValues[^1]} : {world.worldStats.deathCount}\n";
        
        str += "Genome = [";
        str += string.Join(", ", genome);
        str += "]\n";
        return str;
    }
}