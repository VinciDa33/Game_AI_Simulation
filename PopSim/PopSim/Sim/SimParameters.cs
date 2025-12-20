using PopSim.States;
using PopSim.Utility;

namespace PopSim.Sim;

public class SimParameters
{
    private static SimParameters? instance;

    public static SimParameters Instance
    {
        get
        {
            if (instance == null)
                instance = new SimParameters();
            return instance;
        }
    }
    
    //Population
    public int populationSize = 14000;
    public IntRange rangeOfFamilyMembers = new IntRange(1, 5);
    public IntRange rangeOfSocialMembers = new IntRange(2, 8);
    public IntRange rangeOfWorkMembers = new IntRange(4, 16);
    
    //Population Behaviour
    public SocialState[] baseSchedule = [
        SocialState.HOME, //0.00 - 1.00
        SocialState.HOME, //1.00 - 2.00
        SocialState.HOME, //2.00 - 3.00
        SocialState.HOME, //3.00 - 4.00
        SocialState.HOME, //4.00 - 5.00
        SocialState.HOME, //5.00 - 6.00
        SocialState.HOME, //6.00 - 7.00
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
        SocialState.SOCIAL, //18.00 - 19.00
        SocialState.HOME, //19.00 - 20.00
        SocialState.HOME, //20.00 - 21.00
        SocialState.HOME, //21.00 - 22.00
        SocialState.HOME, //22.00 - 23.00
        SocialState.HOME, //23.00 - 24.00
    ];
    
    //Population Weekend Behaviour
    public SocialState[] baseWeekendSchedule = [
        SocialState.HOME, //0.00 - 1.00
        SocialState.HOME, //1.00 - 2.00
        SocialState.HOME, //2.00 - 3.00
        SocialState.HOME, //3.00 - 4.00
        SocialState.HOME, //4.00 - 5.00
        SocialState.HOME, //5.00 - 6.00
        SocialState.HOME, //6.00 - 7.00
        SocialState.HOME, //7.00 - 8.00
        SocialState.HOME, //8.00 - 9.00
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
        SocialState.HOME, //23.00 - 24.00
    ];
    
    //Population remote work Behaviour
    public SocialState[] remoteSchedule = [
        SocialState.HOME, //0.00 - 1.00
        SocialState.HOME, //1.00 - 2.00
        SocialState.HOME, //2.00 - 3.00
        SocialState.HOME, //3.00 - 4.00
        SocialState.HOME, //4.00 - 5.00
        SocialState.HOME, //5.00 - 6.00
        SocialState.HOME, //6.00 - 7.00
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
        SocialState.HOME, //23.00 - 24.00
    ];
    
    
    //Infection
    public int numberOfInitialInfections = 50;
    public float infectionChancePerHour = 0.07f; //7%
    public int meanTimeFromInfectionToSymptomatic = 120; //In hours
    //public int meanTimeFromSymptomaticToDeadly = 168; //In hours
    public float chanceOfDeath = 0.014f;
    
    public float chanceOfRecoveryFromInfectionPerHour = 0.04f; //4%
    public float chanceOfRecoveryFromSymptomaticPerHour = 0.025f; //2.5%

    public float chanceOfVaccinationPerHour = 0.005f; //0.5%. Only applies if the vaccine policy is enabled. 
    public float resistanceGainFromRecovery = 0.7f;
    public float resistanceGainFromVaccine = 0.85f;
    public float resistanceDropOffPerHour = 0.99f; //Resistance is multiplied by this number every hour
}