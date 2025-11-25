using PopSim.Sim;
using PopSim.States;

namespace PopSim.World;

public class SimWorld
{
    public float happiness { get; private set; }
    
    public int day { get; private set; } = 0;
    public int hour { get; private set; } = 0;
    public bool isWeekend { get; private set; } = false;

    public WorldStats worldStats { get; private set; }
    public WorldParameters worldParameters { get; private set; }
    public List<Person> population { get; private set; } = new List<Person>();
    
    public SimWorld()
    {
        worldStats = new WorldStats(this);
        worldParameters = new WorldParameters();
    }

    public void InitWorld()
    {
        happiness = 100 * SimParameters.Instance.populationSize;
        
        for (int i = 0; i < SimParameters.Instance.populationSize; i++)
            population.Add(new Person());
        
        
        foreach (Person p in population)
            p.SetRelations(this);
        
        Random random = new Random();
        for (int i = 0; i < SimParameters.Instance.numberOfInitialInfections; i++)
            population[random.Next(0, population.Count)].Infect();
    }
    
    public void Step(int timeStep)
    {
        foreach (Person p in population)
        {
            p.Step(this, timeStep);
            if (p.healthState == HealthState.SYMPTOMATIC)
                happiness -= 10;
            if (p.healthState == HealthState.DEAD)
                happiness -= 50;
        }


        if (worldParameters.policyStates[0])
            SimParameters.Instance.infectionChancePerHour *= 0.99f;
        
        if (worldParameters.policyStates[1])
            SimParameters.Instance.infectionChancePerHour *= 0.9f;
        
        if (worldParameters.policyStates[2])
            SimParameters.Instance.infectionChancePerHour *= 0.9f;
        
        hour++;
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }

        if (day % 7 == 0 || day % 7 == 6)
            isWeekend = true;
        else
            isWeekend = false;
    }
}