namespace PopSim.Genetic_Algorithm;

public class AlgorithmParameters
{
    private static AlgorithmParameters? instance;

    public static AlgorithmParameters Instance
    {
        get
        {
            if (instance == null)
                instance = new AlgorithmParameters();
            return instance;
        }
    }

    public int simDuration { get; private set; } = 4320;
}