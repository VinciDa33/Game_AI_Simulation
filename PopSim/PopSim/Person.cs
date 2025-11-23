using PopSim.Sim;
using PopSim.States;
using PopSim.Utility;
using PopSim.World;

namespace PopSim;

public class Person
{
    public HealthState healthState { get; private set; }= HealthState.HEALTHY;
    public SocialState socialState { get; private set; }  = SocialState.HOME;
    private int healthStateChangeTimeStep = 0;
    
    public List<Person> familyRealtions { get; private set; } = new List<Person>();
    public List<Person> socialRelations { get; private set; } = new List<Person>();
    public List<Person> workRelations { get; private set; } = new List<Person>();

    public void Step(SimWorld world, int timeStep)
    {
        //We do not need to do anything for dead people
        if (healthState == HealthState.DEAD)
            return;
        
        UpdateSocialState(world.hour, world.isWeekend);
        UpdateInfectedState(timeStep);
        UpdateHealth(timeStep);
        
    }

    private void UpdateHealth(int timeStep)
    {
        if (healthState == HealthState.INFECTED)
        {
            if (RandomManager.Instance.GetNextDouble() < SimParameters.Instance.chanceOfRecoveryFromInfectionPerHour)
            {
                healthState = HealthState.RECOVERED;
                healthStateChangeTimeStep = timeStep;
                return;
            }
            
            if (timeStep >= healthStateChangeTimeStep + SimParameters.Instance.meanTimeFromInfectionToSymptomatic)
            {
                healthState = HealthState.SYMPTOMATIC;
                healthStateChangeTimeStep = timeStep;
            }
        }
        
        else if (healthState == HealthState.SYMPTOMATIC)
        {
            if (RandomManager.Instance.GetNextDouble() < SimParameters.Instance.chanceOfRecoveryFromSymptomaticPerHour)
            {
                healthState = HealthState.RECOVERED;
                healthStateChangeTimeStep = timeStep;
                return;
            }
            
            if (timeStep >= healthStateChangeTimeStep + SimParameters.Instance.meanTimeFromSymptomaticToDeath)
            {
                healthState = HealthState.DEAD;
                healthStateChangeTimeStep = timeStep;
            }
        }
    }
    
    private void UpdateInfectedState(int timeStep)
    {
        //If a person is already infected, we skip em
        if (StateHelper.IsInfectious(healthState))
            return;
        
        //If a person has recovered and antibodies cause immunity, we skip em
        if (healthState == HealthState.RECOVERED && SimParameters.Instance.immuneWithAntibodies)
            return;

        //If a person is at home or sleeping, they can be infected by their family
        if (socialState == SocialState.SLEEPING || socialState == SocialState.HOME)
        {
            foreach (Person p in familyRealtions)
            {
                if (TryInfectByPerson(p, timeStep))
                    return; //The person was infected, and we can stop early
            }
        }
        
        //If a person is at work, they can be infected by their work relations
        else if (socialState == SocialState.WORK)
        {
            foreach (Person p in workRelations)
            {
                if (TryInfectByPerson(p, timeStep))
                    return; //The person was infected, and we can stop early
            }
        }

        //If a person is socialising, they can be infected by their social group
        else if (socialState == SocialState.SOCIAL)
        {
            foreach (Person p in socialRelations)
            {
                if (TryInfectByPerson(p, timeStep))
                    return; //The person was infected, and we can stop early
            }
        }
    }

    private bool TryInfectByPerson(Person infector, int timeStep)
    {
        //Nothing happens if the infector person is not infectious
        if (!StateHelper.IsInfectious(infector.healthState))
            return false;

        //Check if the person becomes infected
        if (RandomManager.Instance.GetNextDouble() < SimParameters.Instance.infectionChancePerHour)
        {
            //The person was infected
            healthState = HealthState.INFECTED;
            healthStateChangeTimeStep = timeStep;
            return true;
        }

        //The person was not infected
        return false;
    }

    private void UpdateSocialState(int hour, bool isWeekend)
    {
        if (SimParameters.Instance.policiesList[4])
        {
            socialState = SimParameters.Instance.remoteSchedule[hour];
        }
        
        //A hospitalized person will stay hospitalized until recovery or death!
        if (socialState == SocialState.HOSPITALIZED)
            return;
        
        
        //If a person is symptomatic, they will always sleep or stay home 
        if (healthState == HealthState.SYMPTOMATIC && SimParameters.Instance.policiesList[5] )
        {
            socialState = SimParameters.Instance.baseSchedule[hour] == SocialState.SLEEPING ? SocialState.SLEEPING : SocialState.HOME;
            return;
        }
        
        
        //Otherwise, while healthy and infected they will follow the base schedule
        socialState = isWeekend ? SimParameters.Instance.baseWeekendSchedule[hour] : SimParameters.Instance.baseSchedule[hour];
    }

    public void SetRelations(SimWorld world)
    { 
        RandomManager random = RandomManager.Instance;
        
        
        byte familyRelationsCount = (byte) SimParameters.Instance.rangeOfFamilyMembers.GetRandomInRange();
        byte socialRelationsCount = (byte) SimParameters.Instance.rangeOfSocialMembers.GetRandomInRange();
        byte workRelationsCount = (byte) SimParameters.Instance.rangeOfWorkMembers.GetRandomInRange();

        while (familyRealtions.Count < familyRelationsCount)
        {
            //If this person has already gotten family relations, skip
            if (familyRealtions.Count != 0)
                break;
            
            //Pick a random person, and discard if it is this person, or already in family relations
            Person p = world.population[random.GetNextInt(world.population.Count)];
            if (p == this || familyRealtions.Contains(p))
                continue;

            foreach (Person f in familyRealtions)
            {
                p.AddFamilyMember(f);
                f.AddFamilyMember(p);
            }
            familyRealtions.Add(p);
            p.AddFamilyMember(this);
        }
        
        while (socialRelations.Count < socialRelationsCount)
        {
            Person p = world.population[random.GetNextInt(world.population.Count)];
            if (p == this || socialRelations.Contains(p))
                continue;
            
            socialRelations.Add(p);
            p.AddSocialMember(this);
        }
        
        while (workRelations.Count < workRelationsCount)
        {
            if (workRelations.Count != 0)
                break;
            
            Person p = world.population[random.GetNextInt(world.population.Count)];
            if (p == this || workRelations.Contains(p))
                continue;
            
            foreach (Person f in workRelations)
            {
                p.AddWorkMember(f);
                f.AddWorkMember(p);
            }
            workRelations.Add(p);
            p.AddWorkMember(this);
        }
        
    }

    public void Infect()
    {
        healthState = HealthState.INFECTED;
    }

    public void AddFamilyMember(Person p)
    {
        familyRealtions.Add(p);
    } 
    public void AddSocialMember(Person p)
    {
        socialRelations.Add(p);
    } 
    public void AddWorkMember(Person p)
    {
        workRelations.Add(p);
    } 
}