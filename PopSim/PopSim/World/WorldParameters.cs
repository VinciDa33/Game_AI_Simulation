using PopSim.Sim;

namespace PopSim.World;

public class WorldParameters
{
    public bool[] policyStates;
    public float infectionChancePerHour;

    public int meanTimeFromInfectionToSymptomatic;
    public int meanTimeFromSymptomaticToDeath;

    public float chanceOfRecoveryFromInfectionPerHour;
    public float chanceOfRecoveryFromSymptomaticPerHour;

    public bool immuneWithAntibodies;
    
    public WorldParameters()
    {
        policyStates = new bool[SimParameters.Instance.policiesList.Count];
        infectionChancePerHour = SimParameters.Instance.infectionChancePerHour;

        meanTimeFromInfectionToSymptomatic = SimParameters.Instance.meanTimeFromInfectionToSymptomatic;
        meanTimeFromSymptomaticToDeath = SimParameters.Instance.meanTimeFromSymptomaticToDeath;

        chanceOfRecoveryFromInfectionPerHour = SimParameters.Instance.chanceOfRecoveryFromInfectionPerHour;
        chanceOfRecoveryFromSymptomaticPerHour = SimParameters.Instance.chanceOfRecoveryFromSymptomaticPerHour;

        immuneWithAntibodies = SimParameters.Instance.immuneWithAntibodies;
    }
}