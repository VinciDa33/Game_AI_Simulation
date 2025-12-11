using PopSim.Genetic_Algorithm;

namespace PopSim.Utility;

public class DeathComparator : Comparer<GeneticAgent> {
    public override int Compare(GeneticAgent a, GeneticAgent b)
    {
        if (a.cumulativeDeathCount[^1] == b.cumulativeDeathCount[^1])
            return 0;
        if (a.cumulativeDeathCount[^1] < b.cumulativeDeathCount[^1])
            return 1;
        return -1;
    }
}