using PopSim.Genetic_Algorithm;

namespace PopSim.World;

public class WorldStatistics
{
    private SimWorld world;
    private bool extensiveLogging;
    
    public int[] cumulativeDeathCount;
    public int[] happinessOverTime;
    
    //Extensive data
    public int[] deathRateOverTime;

    
    
    public WorldStatistics(SimWorld world, bool extensiveLogging)
    {
        this.world = world;
        cumulativeDeathCount = new int[AlgorithmParameters.Instance.simDuration];
        happinessOverTime = new int[AlgorithmParameters.Instance.simDuration];
        
        deathRateOverTime = new int[AlgorithmParameters.Instance.simDuration];

        this.extensiveLogging = extensiveLogging;
    }
    
    public void Update(int timestep)
    {
        //Add the newest death count to the list of cumulative deaths
        cumulativeDeathCount[timestep] = world.worldState.deathCount;
        happinessOverTime[timestep] = world.worldState.happiness;
        
        //Taking the current time steps death count and subtracting the previous time steps death count
        deathRateOverTime[timestep] = world.worldState.deathCount - deathRateOverTime[^1];
    }
}