using PopSim.Genetic_Algorithm;

namespace PopSim.Utility;

public class CrowdingComparator : Comparer<GeneticAgent> {
    public override int Compare(GeneticAgent a, GeneticAgent b) {
        if (a.crowdingDistance==b.crowdingDistance) 
            return 0;
        if (a.crowdingDistance > b.crowdingDistance)
            return 1;
        return -1;
    }
}