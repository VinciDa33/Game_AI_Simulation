using PopSim.Sim;
using PopSim.Utility;
using PopSim.World;

namespace PopSim.Genetic_Algorithm;

public class Algorithm
{
    private List<bool> genome = new List<bool>();
    private SimWorld world;
    public int timeStep = 0;
    
    //Fitness values
    public List<int> infectionValues;
    public List<int> deathValues;
    public List<int> happinessValues;

    public Algorithm(List<bool> genome)
    {
        this.genome = genome;
        world = new SimWorld();
    }
    
    public void Start(List<bool> G)
    {
        world.InitWorld();
        genome = G;
        
        while (true)
        {
            Step();
        }
    }
    
    public void Step()
    {
        world.Step(timeStep);
        int[] temp = fitness();
        infectionValues.Add(temp[3]);
        deathValues.Add(temp[2]);
        happinessValues.Add(temp[1]);
        timeStep++;
    }
    

    public int[] fitness()
    {
        if (genome.Count != SimParameters.Instance.policiesList.Count)
        {
            Console.WriteLine("Policies count mismatch");
            return [0,0,0];
        }
        
        //Alternate (world.worldStats.infectedCount == 0 && world.worldStats.symptomaticCount == 0) 
        world.worldStats.UpdateStats();
        if (timeStep == 8766)
        {
            return [world.worldStats.happiness, world.worldStats.deathCount, world.worldStats.infectedCount];
        }

        for (int i = 0; i < SimParameters.Instance.policiesList.Count; i++)
        {
            SimParameters.Instance.policiesList[i] = genome[i];
        }
        
        return [world.worldStats.happiness, world.worldStats.deathCount, world.worldStats.infectedCount];
    }
    
    
}