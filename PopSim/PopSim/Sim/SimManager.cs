
using PopSim.World;

namespace PopSim.Sim;

public class SimManager
{
    public SimWorld world = new SimWorld();
    public int timeStep = 0;

    public void Start()
    {
        Console.WriteLine("Starting...");
        world.InitWorld();
        
        Console.WriteLine("SimWorld initialized...");
        
        Thread.Sleep(1500);
        
        while (true)
        {
            Step();
            Thread.Sleep(500);
        }
    }
    
    public void Step()
    {
        world.Step();

        timeStep++;
        
        PrintStatus();
    }

    public void PrintStatus()
    {
        WorldStats stats = world.worldStats;
        stats.UpdateStats();
        Console.Clear();
        Console.WriteLine("Time Step ["+ timeStep +"]");
        Console.WriteLine("Days.........." + world.day);
        Console.WriteLine("Time of Day..." + world.hour + ":00");
        Console.WriteLine("Healthy:......" + stats.healthyCount);
        Console.WriteLine("Infected:....." + stats.infectedCount);
        Console.WriteLine("Symptomatic..." + stats.symptomaticCount);
        Console.WriteLine("Dead:........." + stats.deathCount);
        Console.WriteLine("Recovered:...." + stats.recovered);
    }
}