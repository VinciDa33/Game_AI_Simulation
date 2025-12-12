namespace PopSim.Genetic_Algorithm;

public class ParetoSubject
{
    public Agent agent;
    public int dominatedCount = 0;

    public ParetoSubject(Agent agent)
    {
        this.agent = agent;
    }
}