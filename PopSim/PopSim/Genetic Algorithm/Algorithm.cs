using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class Algorithm
{
    public List<bool> genome = new List<bool>();
    private SimWorld world;
    public int timeStep = 0;
    
    //Fitness values
    public List<int> infectionValues = new List<int>();
    public List<int> deathValues = new List<int>();
    public List<int> happinessValues = new List<int>();

    //Averages
    private int infectionAverage {get; set;}
    private int deathAverage {get; set;}
    private int happinessAverage {get; set;}
    
    
    public Algorithm(List<bool> genome)
    {
        this.genome = genome;
        world = new SimWorld();
    }
    
    public void Start()
    {
        world.InitWorld();
        
        while (true)
        {
            Step();
            if (timeStep >= 100)
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
        fitness();
        timeStep++;
    }
    

    public void fitness()
    {
        if (genome.Count != SimParameters.Instance.policiesList.Count)
        {
            throw new Exception("Policies count mismatch");
        }

        for (int i = 0; i < SimParameters.Instance.policiesList.Count; i++)
        {
            SimParameters.Instance.policiesList[i] = genome[i];
        }
        
        //infectionValues.Add(world.worldStats.infectedCount);
        deathValues.Add(world.worldStats.deathCount);
        happinessValues.Add(world.worldStats.happiness);
    }

    public int value()
    {
        return happinessAverage - deathAverage;
    }
}