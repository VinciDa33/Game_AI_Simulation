namespace PopSim;

public static class SimStats
{
    public static int deathCount = 0;
    public static int symptomaticCount = 0;
    public static int infectedCount = 0;
    public static int healthyCount = 0;
    public static int recovered = 0;

    public static void UpdateStats()
    {
        List<Person> population = World.Instance.population;

        deathCount = 0;
        symptomaticCount = 0;
        infectedCount = 0;
        healthyCount = 0;
        recovered = 0;
        
        
        foreach (Person p in population)
        {
            switch (p.state)
            {
                case PersonState.DEAD:
                    deathCount++;
                    break;
                case PersonState.INFECTED:
                    infectedCount++;
                    break;
                case PersonState.SYMPTOMATIC:
                    symptomaticCount++;
                    break;
                case PersonState.HEALTHY:
                    healthyCount++;
                    break;
                case PersonState.RECOVERED:
                    recovered++;
                    break;
            }
        }
    }
}