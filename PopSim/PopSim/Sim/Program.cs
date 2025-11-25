using PopSim.Genetic_Algorithm;
using PopSim.Sim;

namespace PopSim;

class Program
{
    public static void Main(string[] args)
    {
        AlgorithmManager manager = new AlgorithmManager();
        //SimManager manager = new SimManager();
        manager.Start();
    }
}