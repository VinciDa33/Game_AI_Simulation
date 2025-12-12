
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
        WorldState state = world.worldState;
        int popSize = SimParameters.Instance.populationSize;
        state.UpdateState();
        Console.Clear();
        Console.WriteLine("Time Step ["+ timeStep +"]");
        Console.WriteLine("Days.........." + world.day + (world.isWeekend ? " [Weekend]" : ""));
        Console.WriteLine("Time of Day..." + world.hour + ":00");
        Console.WriteLine("Healthy:......" + state.healthyCount + " | " + (state.healthyCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Infected:....." + state.infectedCount + " | " + (state.infectedCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Symptomatic..." + state.symptomaticCount + " | " + (state.symptomaticCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Dead:........." + state.deathCount + " | " + (state.deathCount / (float) popSize) * 100f + "%");
        Console.WriteLine("Recovered:...." + state.recovered  + " | " + (state.recovered / (float) popSize) * 100f + "%");
    }
}