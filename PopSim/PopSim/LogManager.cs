using PopSim.Genetic_Algorithm;

namespace PopSim;

public class LogManager
{
    private static LogManager? instance;
    
    public static LogManager Instance
    {
        get
        {
            if (instance == null)
                instance = new LogManager();
            return instance;
        }
    }
    
    public List<Algorithm> genomesToLog = new List<Algorithm>();
    
    public void Log()
    {
        TextWriter? tw = null;
        try
        {
            tw = new StreamWriter(File.OpenWrite("Log.txt"));
            foreach (Algorithm pop in genomesToLog)
            {
                tw.WriteLine(pop);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Oh no.\n" + e.Message);
        }
        finally
        {
            tw?.Close();
        }
    }
    
    
}