namespace PopSim.Utility;

public class IntRange
{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
    
    public int GetRandomInRange()
    {
        Random random = new Random();
        return random.Next(min, max + 1);
    }
}