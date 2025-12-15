using PopSim.Genetic_Algorithm;

namespace PopSim.Sorting;

public class CrowdingComparator : Comparer<Agent>
{
    private Dictionary<Agent, double> crowdingDistances;
    
    public CrowdingComparator(Dictionary<Agent, double> crowdingDistances)
    {
        this.crowdingDistances = crowdingDistances;
    }
    
    public override int Compare(Agent a, Agent b) {
        if (crowdingDistances[a] == crowdingDistances[b]) 
            return 0;
        if (crowdingDistances[a]  > crowdingDistances[b])
            return 1;
        return -1;
    }
}