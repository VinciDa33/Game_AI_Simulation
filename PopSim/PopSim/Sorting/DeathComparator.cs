using PopSim.Genetic_Algorithm;

namespace PopSim.Sorting;

public class DeathComparator : Comparer<Agent> {
    public override int Compare(Agent a, Agent b)
    {
        if (a.GetFitnessDeathCount() == b.GetFitnessDeathCount())
            return 0;
        
        //Remember we are minimizing death count!
        if (a.GetFitnessDeathCount() < b.GetFitnessDeathCount())
            return 1;
        return -1;
    }
}