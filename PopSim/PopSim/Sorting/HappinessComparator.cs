using PopSim.Genetic_Algorithm;

namespace PopSim.Sorting;

public class HappinessComparator : Comparer<Agent> {
    public override int Compare(Agent a, Agent b) {
        if (a.GetFitnessHappiness() == b.GetFitnessHappiness())
            return 0;
        
        //Remember we are maximizing happiness
        if (a.GetFitnessHappiness() > b.GetFitnessHappiness())
            return 1;
        return -1;
    }
}