using PopSim.Genetic_Algorithm;

namespace PopSim.Sim;

class Program
{
    public static void Main(string[] args)
    {
        EvolutionManager manager = new EvolutionManager();
        //SimManager manager = new SimManager();
        manager.Start();
    }
}