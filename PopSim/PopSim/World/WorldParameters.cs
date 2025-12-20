using PopSim.Policies;
using PopSim.Sim;

namespace PopSim.World;

public class WorldParameters
{
    public float infectionChancePerHour;

    public int meanTimeFromInfectionToSymptomatic;
    public int meanTimeFromSymptomaticToDeadly = 168; //In hours
    public float chanceOfDeath = 0.0412f;
    
    public float chanceOfRecoveryFromInfectionPerHour;
    public float chanceOfRecoveryFromSymptomaticPerHour;
    
    public WorldParameters()
    {
        infectionChancePerHour = SimParameters.Instance.infectionChancePerHour;

        meanTimeFromInfectionToSymptomatic = SimParameters.Instance.meanTimeFromInfectionToSymptomatic;
        meanTimeFromSymptomaticToDeadly = SimParameters.Instance.meanTimeFromSymptomaticToDeadly;
        chanceOfDeath = SimParameters.Instance.chanceOfDeath;

        chanceOfRecoveryFromInfectionPerHour = SimParameters.Instance.chanceOfRecoveryFromInfectionPerHour;
        chanceOfRecoveryFromSymptomaticPerHour = SimParameters.Instance.chanceOfRecoveryFromSymptomaticPerHour;
    }
}