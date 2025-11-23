using PopSim.Sim;

namespace PopSim.World;

public class SimWorld
{
    public float happiness { get; private set; }
    
    public int day { get; private set; } = 0;
    public int hour { get; private set; } = 0;
    public bool isWeekend { get; private set; } = false;

    public WorldStats worldStats;
    public List<Person> population { get; private set; } = new List<Person>();
    
    public SimWorld()
    {
        worldStats = new WorldStats(this);
    }

    public void InitWorld()
    {
        Console.WriteLine("Creating population...");
        for (int i = 0; i < SimParameters.Instance.populationSize; i++)
            population.Add(new Person());
        
        Console.WriteLine("Setting relations...");
        foreach (Person p in population)
            p.SetRelations(this);
        
        Console.WriteLine("Infecting patient Zero...");
        Random random = new Random();
        for (int i = 0; i < SimParameters.Instance.numberOfInitialInfections; i++)
            population[random.Next(0, population.Count)].Infect();
    }
    
    public void Step(int timeStep)
    {
        foreach (Person p in population)
            p.Step(this, timeStep);

        if (SimParameters.Instance.policiesList[0] == true)
            SimParameters.Instance.infectionChancePerHour *= 0.99f;
        
        if (SimParameters.Instance.policiesList[1] == true)
            SimParameters.Instance.infectionChancePerHour *= 0.9f;
        
        if (SimParameters.Instance.policiesList[2] == true)
            SimParameters.Instance.infectionChancePerHour *= 0.9f;
        
        if (SimParameters.Instance.policiesList[3] == true)
            
        
        
        
        
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