using PopSim.States;

namespace PopSim.World;

public class WorldState
{
    private SimWorld world;
    public int deathCount { get; private set; } = 0;
    public int symptomaticCount { get; private set; } = 0;
    public int infectedCount { get; private set; } = 0;
    public int healthyCount { get; private set; } = 0;
    public int recovered { get; private set; } = 0;
    public long happiness;
    
    public WorldState(SimWorld world)
    {
        this.world = world;
        healthyCount = world.population.Count;
    }
    
    public void UpdateState()
    {
        List<Person> population = world.population;

        deathCount = 0;
        symptomaticCount = 0;
        infectedCount = 0;
        healthyCount = 0;
        recovered = 0;
        
        
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