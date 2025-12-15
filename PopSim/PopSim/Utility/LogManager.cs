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
    
    public List<string> dataToLog = new List<string>();
    
    public void Log()
    {
        Console.WriteLine("Writing Logs!");
        TextWriter? tw = null;
        try
        {
            tw = new StreamWriter(File.Create("Log.txt"));
            foreach (string data in dataToLog)
            {
                tw.WriteLine(data);
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