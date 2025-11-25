using PopSim.Sim;
using PopSim.States;

namespace PopSim.World;

public class SimWorld
{
    public int happiness;
    
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
        }
        
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