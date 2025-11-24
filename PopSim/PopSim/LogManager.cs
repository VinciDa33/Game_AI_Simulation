using PopSim.Genetic_Algorithm;

namespace PopSim;

public class LogManager
{
    private static LogManager instance;
    
    public List<Algorithm> bestGenomes = new List<Algorithm>();
    
    public static LogManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LogManager();
            }
            return instance;
        }
    }
    
    public void Log()
    {
        foreach (Algorithm pop in bestGenomes)
        {
            string genomeString = string.Join(", ", pop.genome);
            string log = $"Generation {bestGenomes.Count}: Fitness = {pop.Fitnessvalue()}: Genome = {genomeString}";
            File.AppendAllText("log.txt", log);
        }
    }
    
    
}