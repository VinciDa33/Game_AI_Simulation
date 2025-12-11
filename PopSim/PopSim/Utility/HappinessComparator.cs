using PopSim.Genetic_Algorithm;

namespace PopSim.Utility;

public class HappinessComparator : Comparer<GeneticAgent> {
    public override int Compare(GeneticAgent a, GeneticAgent b) {
        if (a.happinessAverage == b.happinessAverage)
            return 0;
        if (a.happinessAverage > b.happinessAverage)
            return 1;
        return -1;
    }
}