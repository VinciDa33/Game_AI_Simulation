namespace PopSim.Genetic_Algorithm;

public class ParetoFronts
{
    public List<List<Agent>> fronts { get; private set; } = [];

    public void AddFront(List<Agent> newFront)
    {
        fronts.Add(newFront);
    }

    public int GetFrontRank(Agent agent)
    {
        for (int i = 0; i < fronts.Count; i++) //Run through each front
            for (int j = 0; j < fronts[i].Count; j++) //Run through each agent in front
                if (fronts[i][j] == agent) //If it matches the parameter return the front rank
                    return i;
        
        return int.MaxValue;
    }
}