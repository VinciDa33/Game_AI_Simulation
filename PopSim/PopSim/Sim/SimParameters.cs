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
    public int populationSize = 5500;
    public IntRange rangeOfFamilyMembers = new IntRange(1, 5);
    public IntRange rangeOfSocialMembers = new IntRange(2, 8);
    public IntRange rangeOfWorkMembers = new IntRange(4, 16);
    
    //Population Behaviour
    public SocialState[] baseSchedule = [
        SocialState.SLEEPING, //0.00 - 1.00
        SocialState.SLEEPING, //1.00 - 2.00
        SocialState.SLEEPING, //2.00 - 3.00
        SocialState.SLEEPING, //3.00 - 4.00
        SocialState.SLEEPING, //4.00 - 5.00
        SocialState.SLEEPING, //5.00 - 6.00
        SocialState.SLEEPING, //6.00 - 7.00
        SocialState.HOME, //7.00 - 8.00
        SocialState.WORK, //8.00 - 9.00
        SocialState.WORK, //9.00 - 10.00
        SocialState.WORK, //10.00 - 11.00
        SocialState.WORK, //11.00 - 12.00
        SocialState.WORK, //12.00 - 13.00
        SocialState.WORK, //13.00 - 14.00
        SocialState.WORK, //14.00 - 15.00
        SocialState.SOCIAL, //15.00 - 16.00
        SocialState.SOCIAL, //16.00 - 17.00
        SocialState.SOCIAL, //17.00 - 18.00
        SocialState.HOME, //18.00 - 19.00
        SocialState.HOME, //19.00 - 20.00
        SocialState.HOME, //20.00 - 21.00
        SocialState.HOME, //21.00 - 22.00
        SocialState.HOME, //22.00 - 23.00
        SocialState.SLEEPING, //23.00 - 24.00
    ];
    
    //Population Weekend Behaviour
    public SocialState[] baseWeekendSchedule = [
        SocialState.SLEEPING, //0.00 - 1.00
        SocialState.SLEEPING, //1.00 - 2.00
        SocialState.SLEEPING, //2.00 - 3.00
        SocialState.SLEEPING, //3.00 - 4.00
        SocialState.SLEEPING, //4.00 - 5.00
        SocialState.SLEEPING, //5.00 - 6.00
        SocialState.SLEEPING, //6.00 - 7.00
        SocialState.SLEEPING, //7.00 - 8.00
        SocialState.SLEEPING, //8.00 - 9.00
        SocialState.SOCIAL, //9.00 - 10.00
        SocialState.SOCIAL, //10.00 - 11.00
        SocialState.SOCIAL, //11.00 - 12.00
        SocialState.SOCIAL, //12.00 - 13.00
        SocialState.SOCIAL, //13.00 - 14.00
        SocialState.SOCIAL, //14.00 - 15.00
        SocialState.SOCIAL, //15.00 - 16.00
        SocialState.SOCIAL, //16.00 - 17.00
        SocialState.SOCIAL, //17.00 - 18.00
        SocialState.SOCIAL, //18.00 - 19.00
        SocialState.HOME, //19.00 - 20.00
        SocialState.HOME, //20.00 - 21.00
        SocialState.HOME, //21.00 - 22.00
        SocialState.HOME, //22.00 - 23.00
        SocialState.SLEEPING, //23.00 - 24.00
    ];
    
    //Population remote work Behaviour
    public SocialState[] remoteSchedule = [
        SocialState.SLEEPING, //0.00 - 1.00
        SocialState.SLEEPING, //1.00 - 2.00
        SocialState.SLEEPING, //2.00 - 3.00
        SocialState.SLEEPING, //3.00 - 4.00
        SocialState.SLEEPING, //4.00 - 5.00
        SocialState.SLEEPING, //5.00 - 6.00
        SocialState.SLEEPING, //6.00 - 7.00
        SocialState.HOME, //7.00 - 8.00
        SocialState.HOME, //8.00 - 9.00
        SocialState.HOME, //9.00 - 10.00
        SocialState.HOME, //10.00 - 11.00
        SocialState.HOME, //11.00 - 12.00
        SocialState.HOME, //12.00 - 13.00
        SocialState.HOME, //13.00 - 14.00
        SocialState.HOME, //14.00 - 15.00
        SocialState.SOCIAL, //15.00 - 16.00
        SocialState.SOCIAL, //16.00 - 17.00
        SocialState.SOCIAL, //17.00 - 18.00
        SocialState.HOME, //18.00 - 19.00
        SocialState.HOME, //19.00 - 20.00
        SocialState.HOME, //20.00 - 21.00
        SocialState.HOME, //21.00 - 22.00
        SocialState.HOME, //22.00 - 23.00
        SocialState.SLEEPING, //23.00 - 24.00
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