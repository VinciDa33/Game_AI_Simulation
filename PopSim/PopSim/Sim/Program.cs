using PopSim.Genetic_Algorithm;

namespace PopSim.Sim;

class Program
{
    public static void Main(string[] args)
    {
        int generation = 3;
        int generationSize = 50;
        int generationCount = 50;

        try
        {
            generation = int.Parse(args[0]);
            generationSize = int.Parse(args[1]);
            generationCount = int.Parse(args[2]);
        }
        catch (Exception e)
        {
            Console.WriteLine("Bad input!");
            return;
        }
        
        
        EvolutionManager manager = new EvolutionManager(generation, generationSize, generationCount);

        try
        {
            manager.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!\n" +
                              "!!!!!!!!!!!!!!!!\n" +
                              "A FATAL ERROR OCCURED, RETRYING\n" +
                              "!!!!!!!!!!!!!!!!\n" +
                              "!!!!!!!!!!!!!!!!\n");
            manager.Start();
        }
    }
}