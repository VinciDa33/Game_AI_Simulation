using PopSim.Genetic_Algorithm;

namespace PopSim;

public class LogManager
{
    public List<string> dataToLog = [];
    
    public void Log(string filename)
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