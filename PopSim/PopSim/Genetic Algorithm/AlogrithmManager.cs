namespace PopSim.Genetic_Algorithm;

public class AlogrithmManager
{
    private List<Algorithm> population = new List<Algorithm>();
    Random random = new Random();
    
    public List<bool> generateGenome(int length)
    {
        List<bool> genome = new List<bool>();
        for (int i = 0; i < length; i++)
        {
            genome.Add(random.Next(0, 2) == 0);
        }
        return genome;
    }
    
    public void generatePop(int size)
    {
        for (int i = 0; i < size; i++)
        {
            population.Add(new Algorithm(generateGenome(9)));
        }
    }

    public List<bool>[] singlePointCrossover(List<bool> genomeA, List<bool> genomeB)
    {
        int p = random.Next(1, genomeA.Count);
        
        List<bool> offspringA = new List<bool>();
        offspringA.AddRange(genomeA.GetRange(0,p));
        offspringA.AddRange(genomeB.GetRange(p, genomeA.Count -p));

        List<bool> offspringB = new List<bool>();
        offspringB.AddRange(genomeB.GetRange(0,p));
        offspringB.AddRange(genomeA.GetRange(p, genomeB.Count -p));
        
        return [offspringA,offspringB];
    }

    public List<bool> mutation(List<bool> genome, int num = 2, double propability = 0.5)
    {
        for (int i = 0; i < 2; i++)
        {
            int r = random.Next(genome.Count);
            if (random.NextDouble() <= propability)
            {
                genome[r] = !genome[r];
            }
        }
        return genome;
    }


    
}