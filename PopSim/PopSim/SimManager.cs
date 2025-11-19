namespace PopSim;

public class SimManager
{
    public World world = World.Instance;
    public int timeStep = 0;
    public int hourOfDay = 0;

    public void Start()
    {
        Console.WriteLine("Starting...");
        world.InitWorld();
        
        Console.WriteLine("World initialized...");
        
        Thread.Sleep(1500);
        
        while (true)
        {
            Step();
            Thread.Sleep(500);
        }
    }
    
    public void Step()
    {
        world.Update(hourOfDay);

        timeStep++;
        hourOfDay++;
        hourOfDay = hourOfDay > 23 ? 0 : hourOfDay;
        
        PrintStatus();
    }

    public void PrintStatus()
    {
        SimStats.UpdateStats();
        Console.Clear();
        Console.WriteLine("Time Step ["+ timeStep +"]");
        Console.WriteLine("Time of Day..." + hourOfDay + ":00");
        Console.WriteLine("Healthy:......" + SimStats.healthyCount);
        Console.WriteLine("Infected:....." + SimStats.infectedCount);
        Console.WriteLine("Symptomatic..." + SimStats.symptomaticCount);
        Console.WriteLine("Dead:........." + SimStats.deathCount);
        Console.WriteLine("Recovered:...." + SimStats.recovered);
    }
}