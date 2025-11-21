using PopSim.States;

namespace PopSim.World;

public class WorldStats
{
    private SimWorld world;
    public int deathCount { get; private set; } = 0;
    public int symptomaticCount { get; private set; } = 0;
    public int infectedCount { get; private set; } = 0;
    public int healthyCount { get; private set; } = 0;
    public int recovered { get; private set; } = 0;
    public int hospitalized { get; private set; } = 0;
    public int happiness { get; private set; } = 0;

    public WorldStats(SimWorld world)
    {
        this.world = world;
        healthyCount = world.population.Count;
    }
    
    public void UpdateStats()
    {
        List<Person> population = world.population;

        deathCount = 0;
        symptomaticCount = 0;
        infectedCount = 0;
        healthyCount = 0;
        recovered = 0;
        hospitalized = 0;
        
        
        foreach (Person p in population)
        {
            switch (p.healthState)
            {
                case HealthState.DEAD:
                    deathCount++;
                    break;
                case HealthState.INFECTED:
                    infectedCount++;
                    break;
                case HealthState.SYMPTOMATIC:
                    symptomaticCount++;
                    break;
                case HealthState.HEALTHY:
                    healthyCount++;
                    break;
                case HealthState.RECOVERED:
                    recovered++;
                    break;
            }
        }
    }
}