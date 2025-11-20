using PopSim.Utility;

namespace PopSim.Sim;

public static class SimParameters
{
    //Population
    public static readonly int populationSize = 1000;
    public static readonly IntRange rangeOfFamilyMembers = new IntRange(1, 5);
    public static readonly IntRange rangeOfSocialMembers = new IntRange(2, 8);
    public static readonly IntRange rangeOfWorkMembers = new IntRange(4, 16);
    
    //Population Behaviour
    public static readonly List<IntRange> schedules = [];
    
    //Infection
    public static readonly int numberOfInitialInfections = 10;
    public static readonly float infectionChancePerHour = 0.1f; //10%
    public static readonly int meanTimeFromInfectionToSymptomatic = 10; //In hours
    public static readonly int meanTimeFromSymptomaticToDeath = 24; //In hours

    public static readonly float chanceOfRecoveryFromInfectionPerHour = 0.01f; //1%
    public static readonly float chanceOfRecoveryFromSymptomaticPerHour = 0.005f; //0.5%

    public static readonly bool immuneWithAntibodies = false;
}