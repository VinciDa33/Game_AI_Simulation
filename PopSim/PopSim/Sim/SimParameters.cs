using PopSim.States;
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
    public static readonly List<ScheduleItem> schedule =
    [
        new ScheduleItem(SocialState.SLEEPING, new IntRange(23, 6)),
        new ScheduleItem(SocialState.HOME, new IntRange(7, 7)),
        new ScheduleItem(SocialState.WORK, new IntRange(8, 15)),
        new ScheduleItem(SocialState.SOCIAL, new IntRange(16, 17)),
        new ScheduleItem(SocialState.HOME, new IntRange(18, 22))
    ]; 
    
    //Hospitals
    public static readonly int maxHospitalCapacity = 150;
    
    //Infection
    public static readonly int numberOfInitialInfections = 10;
    public static readonly float infectionChancePerHour = 0.1f; //10%
    public static readonly int meanTimeFromInfectionToSymptomatic = 10; //In hours
    public static readonly int meanTimeFromSymptomaticToDeath = 24; //In hours

    public static readonly float chanceOfRecoveryFromInfectionPerHour = 0.01f; //1%
    public static readonly float chanceOfRecoveryFromSymptomaticPerHour = 0.005f; //0.5%

    public static readonly bool immuneWithAntibodies = false;
}