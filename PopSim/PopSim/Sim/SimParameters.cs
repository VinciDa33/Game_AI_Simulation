using PopSim.States;
using PopSim.Utility;

namespace PopSim.Sim;

public class SimParameters
{
    private static SimParameters instance;

    public static SimParameters Instance
    {
        get
        {
            if (instance == null)
                instance = new SimParameters();
            return instance;
        }
    }

    private SimParameters()
    {
        policiesList = new List<bool> {awareness, sanitise, mask, remote, isolation, sLockdown, tLockdown, vaccine};
    }
    
    
    //Population
    public int populationSize = 550000;
    public IntRange rangeOfFamilyMembers = new IntRange(1, 5);
    public IntRange rangeOfSocialMembers = new IntRange(2, 8);
    public IntRange rangeOfWorkMembers = new IntRange(4, 16);
    
    //Population Behaviour
    public  List<ScheduleItem> schedule =
    [
        new ScheduleItem(SocialState.SLEEPING, new IntRange(23, 6)),
        new ScheduleItem(SocialState.HOME, new IntRange(7, 7)),
        new ScheduleItem(SocialState.WORK, new IntRange(8, 15)),
        new ScheduleItem(SocialState.SOCIAL, new IntRange(16, 17)),
        new ScheduleItem(SocialState.HOME, new IntRange(18, 22))
    ]; 
    
    //Hospitals
    public  int maxHospitalCapacity = 150;
    
    //policies
    public bool awareness;
    public bool sanitise;
    public bool mask;
    public bool remote;
    public bool isolation;
    public bool sLockdown;
    public bool tLockdown;
    public bool vaccine;

    public List<bool> policiesList;
    
    //Infection
    public int numberOfInitialInfections = 100;
    public float infectionChancePerHour = 0.07f; //7%
    public int meanTimeFromInfectionToSymptomatic = 120; //In hours
    public int meanTimeFromSymptomaticToDeath = 288; //In hours

    public float chanceOfRecoveryFromInfectionPerHour = 0.01f; //1%
    public float chanceOfRecoveryFromSymptomaticPerHour = 0.005f; //0.5%

    public bool immuneWithAntibodies = false;
    
    public void updateNumbers()
    {
        if (awareness)
        {
            infectionChancePerHour = infectionChancePerHour * 0.9f;
        }
        if (sanitise)
        {
            infectionChancePerHour =  infectionChancePerHour * 0.9f;
        }
        if (mask)
        {
            infectionChancePerHour =  infectionChancePerHour * 0.9f;
        }
    }
}