namespace PopSim;

public class World
{
    private static World? instance;
    public static World Instance
    {
        get
        {
            if (instance == null)
                instance = new World();
            return instance;
        }
    }
    private World()
    {
    }

    public List<Person> population { get; private set; } = new List<Person>();

    public void InitWorld()
    {
        Console.WriteLine("Creating population...");
        for (int i = 0; i < SimParameters.populationSize; i++)
            population.Add(new Person());
        SimStats.healthyCount = population.Count;
        
        Console.WriteLine("Setting relations...");
        foreach (Person p in population)
            p.SetRelations();
        
        Console.WriteLine("Infecting patient Zero...");
        Random random = new Random();
        for (int i = 0; i < SimParameters.numberOfInitialInfections; i++)
            population[random.Next(0, population.Count)].Infect();
    }
    
    public void Update(int hour)
    {
        foreach (Person p in population)
        {
            p.Update(hour);
        }
    }
}