namespace PopSim.Utility;

public class RandomManager
{
    private static RandomManager? instance;

    public static RandomManager Instance
    {
        get
        {
            if (instance == null)
                instance = new RandomManager();
            return instance;
        }
    }

    private Random random = new Random();
    
    private RandomManager()
    {
        
    }

    /// <summary>
    /// Double value from 0.0 to less than 1.0
    /// </summary>
    /// <returns></returns>
    public double GetNextDouble()
    {
        return random.NextDouble();
    }

    public double GetNextDouble(double min, double max)
    {
        return min + random.NextDouble() * (max - min);
    }
    
    /// <summary>
    /// Random integer between 0 and integer max
    /// </summary>

    /// <returns></returns>
    
    public int GetNextInt()
    {
        return random.Next(); 
    }

    /// <summary>
    /// Random integer between 0 and max
    /// </summary>
    /// <param name="max">exclusive</param>
    /// <returns></returns>
    
    public int GetNextInt(int max)
    {
        return random.Next(max); 
    }
    
    /// <summary>
    /// Random integer between min and max
    /// </summary>
    /// <param name="min">inclusive</param>
    /// <param name="max">exclusive</param>
    /// <returns></returns>
    
    public int GetNextInt(int min, int max)
    {
        return random.Next(min, max); 
    }
}