using PopSim.Genetic_Algorithm;
using PopSim.States;

namespace PopSim.World;

public class WorldStatistics
{
    private SimWorld world;
    private bool extensiveLogging;
    
    public int[] cumulativeDeathCount;
    public int[] happinessOverTime;
    
    //Extensive data
    public int[] deathRateOverTime;
    public int[] recoveredRateOverTime;
    public float[] immunityPercentageOverTime; //Considered immune at a resistance over 80%

    public int[] infectedCountOverTime;


    
    
    public WorldStatistics(SimWorld world, bool extensiveLogging)
    {
        this.world = world;
        cumulativeDeathCount = new int[AlgorithmParameters.Instance.simDuration];
        happinessOverTime = new int[AlgorithmParameters.Instance.simDuration];
        
        deathRateOverTime = new int[AlgorithmParameters.Instance.simDuration];
        deathRateOverTime[0] = 0;
        
        recoveredRateOverTime = new int[AlgorithmParameters.Instance.simDuration];
        recoveredRateOverTime[0] = 0;

        immunityPercentageOverTime = new float[AlgorithmParameters.Instance.simDuration];

        infectedCountOverTime = new int[AlgorithmParameters.Instance.simDuration];
        
        this.extensiveLogging = extensiveLogging;
    }
    
    public void Update(int timestep)
    {
        //Add the newest death count to the list of cumulative deaths
        cumulativeDeathCount[timestep] = world.worldState.deathCount;
        happinessOverTime[timestep] = world.worldState.happiness;
        
        if (!extensiveLogging)
            return;
        
        //Taking the current time steps death count and subtracting the previous time steps death count
        if (timestep > 0)
            deathRateOverTime[timestep] = world.worldState.deathCount - deathRateOverTime[^1];
            recoveredRateOverTime[timestep] = world.worldState.recovered - recoveredRateOverTime[^1];

        immunityPercentageOverTime[timestep] = CalculateImmunityPercentage();

        infectedCountOverTime[timestep] = world.worldState.infectedCount + world.worldState.symptomaticCount;
    }

    private float CalculateImmunityPercentage()
    {
        int totalAlivePopulationCount = 0;
        int totalImmunePopulationCount = 0;
        
        foreach (Person p in world.population)
        {
            if (p.healthState == HealthState.DEAD)
                continue;
            
            totalAlivePopulationCount += 1;
            
            if (p.resitance >= 0.8f)
                totalImmunePopulationCount += 1;
        }

        return totalImmunePopulationCount / (float) totalAlivePopulationCount;
    }

    public override string ToString()
    {
        string str = "";
        str += "Total death count over time: [";
        str += string.Join(", ", cumulativeDeathCount);
        str += "]\n";
        
        str += "Happiness over time: [";
        str += string.Join(", ", happinessOverTime);
        str += "]\n";
        
        str += "Death rate over time: [";
        str += string.Join(", ", deathRateOverTime);
        str += "]\n";
        
        str += "Recovery rate over time: [";
        str += string.Join(", ", recoveredRateOverTime);
        str += "]\n";
        
        str += "Immunity % over time: [";
        str += string.Join(", ", immunityPercentageOverTime);
        str += "]\n";
        
        str += "Total infected+symptomatic count over time: [";
        str += string.Join(", ", infectedCountOverTime);
        str += "]\n";
        return str;
    }
}