
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
            //Thread.Sleep(250);
        }
    }
    
    public void Step()
    {
        world.Step(timeStep);

        timeStep++;
        
        PrintStatus();
    }

    public void PrintStatus()
    {
        WorldStats stats = world.worldStats;
        int popSize = SimParameters.Instance.populationSize;
        stats.UpdateStats();
        Console.Clear();
        Console.WriteLine("Time Step ["+ timeStep +"]");
        Console.WriteLine("Days.........." + world.day + (world.isWeekend ? " [Weekend]" : ""));
        Console.WriteLine("Time of Day..." + world.hour + ":00");
        Console.WriteLine("Healthy:......" + stats.healthyCount + " | " + (stats.healthyCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Infected:....." + stats.infectedCount + " | " + (stats.infectedCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Symptomatic..." + stats.symptomaticCount + " | " + (stats.symptomaticCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Dead:........." + stats.deathCount + " | " + (stats.deathCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Recovered:...." + stats.recovered  + " | " + (stats.recovered / (float) popSize) * 100f + "%");
    }
}