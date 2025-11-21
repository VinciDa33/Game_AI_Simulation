using Microsoft.VisualBasic.CompilerServices;

namespace PopSim.Genetic_Algorithm;

public class AlgorithmManager
{
    private List<Algorithm> population = new List<Algorithm>();
    Random random = new Random();
    private int generationSize = 10;
    private int generationCap = 100;

    public void Start()
    {
        generatePop(generationSize);
        for (int j = 0; j < generationCap; j++)
        {
            for (int i = 0; i < population.Count; i++)
            {
                population[i].Start();
            }
            Console.WriteLine("Generation finished");
            List<bool>[] offspring = singlePointCrossover(selection());
            List<List<bool>> newPopulation = new List<List<bool>>();
            for (int i = 0; i < generationSize; i++)
            { 
                newPopulation.Add(offspring[i%2]);
            }
        
            for (int i = 0; i < generationSize; i++)
            {
                mutation(newPopulation[i]);
                population[i].genome = newPopulation[i];
            }
        }
        
    }
    
    
    
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
            population.Add(new Algorithm(generateGenome(8)));
        }
    }

    public List<bool>[] singlePointCrossover(Algorithm[] parents)
    {
        List<bool> genomeA = parents[0].genome; 
        List<bool> genomeB = parents[1].genome;
        
        
        int p = random.Next(1, genomeA.Count);
        
        List<bool> offspringA = new List<bool>();
        offspringA.AddRange(genomeA.GetRange(0,p));
        offspringA.AddRange(genomeB.GetRange(p, genomeA.Count -p));

        List<bool> offspringB = new List<bool>();
        offspringB.AddRange(genomeB.GetRange(0,p));
        offspringB.AddRange(genomeA.GetRange(p, genomeB.Count -p));
        
        return [offspringA,offspringB];
    }

    public void mutation(List<bool> genome, int num = 2, double propability = 0.5)
    {
        for (int i = 0; i < num; i++)
        {
            int r = random.Next(genome.Count);
            if (random.NextDouble() <= propability)
            {
                genome[r] = !genome[r];
            }
        }
    }

    public Algorithm[] selection()
    {
        int[] selection = {Int32.MinValue,Int32.MinValue};
        int[] index = new int[2];
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].value() > selection[0])
            {
                selection[1] = selection[0];
                selection[0] = population[i].value();
                index[1] = index[0];
                index[0] = i;
            }else if (population[i].value() > selection[1])
            {
                selection[1] = population[i].value();
                index[1] = i;
            }
        }

        return [ population[index[0]], population[index[1]] ];
    }
    
    
    
}