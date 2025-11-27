using PopSim.Genetic_Algorithm;
using PopSim.Sim;

namespace PopSim;

class Program
{
    public static void Main(string[] args)
    {
        EvolutionManager manager = new EvolutionManager();
        //SimManager manager = new SimManager();
        manager.Start();
    }
}