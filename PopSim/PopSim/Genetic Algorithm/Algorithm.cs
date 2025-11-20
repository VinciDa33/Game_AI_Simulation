using PopSim.Sim;
using PopSim.Utility;

namespace PopSim.Genetic_Algorithm;

public class Algorithm
{
    private List<byte> genome = new List<byte>();
    
    public void generateGenome(List<byte> G)
    {
        Random random = new Random();
        for (int i = 0; i < 9; i++)
        {
            genome.Add((byte)random.Next(0, 1));
        }
    }

    public void fitness()
    {
        if (genome.Count != SimParameters.Instance.policiesList.Count)
        {
            Console.WriteLine("Policies count mismatch");
            return;
        }
        
    }
}