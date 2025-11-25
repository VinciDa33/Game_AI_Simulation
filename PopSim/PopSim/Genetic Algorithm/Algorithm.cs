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
    private int simDuration = 8766;
    
    //Fitness values
    //public List<int> infectionValues = new List<int>();
    public List<int> deathValues = new List<int>();
    public List<int> happinessValues = new List<int>();

    //Averages
    //private int infectionAverage {get; set;}
    private int deathAverage {get; set;}
    private int happinessAverage {get; set;}
    
    
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
            {
                //infectionAverage = infectionValues.Sum()/infectionValues.Count;
                deathAverage = deathValues.Sum()/deathValues.Count;
                happinessAverage = happinessValues.Sum()/happinessValues.Count;
                break;
            }
        }
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
            world.worldParameters.policyStates[i] = genome[i];
        }
        
        //infectionValues.Add(world.worldStats.infectedCount);
        deathValues.Add(world.worldStats.deathCount);
        happinessValues.Add(world.worldStats.happiness);
    }

    public int FitnessValue()
    {
        return happinessAverage - deathAverage;
    }

    public override string ToString()
    {
        string str = $"Generation = {generation}\n Fitness = {FitnessValue()}\n";

        str += "Genome = [";
        str += string.Join(", ", genome);
        str += "]\n";
        return str;
    }
}