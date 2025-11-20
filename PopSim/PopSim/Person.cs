using PopSim.Sim;
using PopSim.States;
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
        if (healthState == HealthState.DEAD)
            return;
        
        UpdateSocialState(world.hour);
        UpdateInfectedState(timeStep);
        UpdateHealth(timeStep);
        
    }

    private void UpdateHealth(int timeStep)
    {
        Random random = new Random();

        if (healthState == HealthState.INFECTED)
        {
            if (random.NextDouble() < SimParameters.Instance.chanceOfRecoveryFromInfectionPerHour)
            {
                healthState = HealthState.RECOVERED;
                healthStateChangeTimeStep = timeStep;
            }
        }

        if (healthState == HealthState.INFECTED)
        {
            if (timeStep >= healthStateChangeTimeStep + SimParameters.Instance.meanTimeFromInfectionToSymptomatic)
            {
                healthState = HealthState.SYMPTOMATIC;
                healthStateChangeTimeStep = timeStep;
            }
        }

        if (healthState == HealthState.SYMPTOMATIC)
        {
            if (random.NextDouble() < SimParameters.Instance.chanceOfRecoveryFromSymptomaticPerHour)
            {
                healthState = HealthState.RECOVERED;
                healthStateChangeTimeStep = timeStep;
                
            }
        }

        if (healthState == HealthState.SYMPTOMATIC)
        {
            if (timeStep >= healthStateChangeTimeStep + SimParameters.Instance.meanTimeFromSymptomaticToDeath)
            {
                healthState = HealthState.DEAD;
                healthStateChangeTimeStep = timeStep;
            }
        }
    }
    
    private void UpdateInfectedState(int timeStep)
    {
        Random random = new Random();
        
        if (healthState == HealthState.RECOVERED && SimParameters.Instance.immuneWithAntibodies)
            return;
        
        if (socialState == SocialState.SLEEPING)
        {
            foreach (Person p in familyRealtions)
            {
                if ((p.healthState == HealthState.INFECTED || p.healthState == HealthState.SYMPTOMATIC) && healthState != HealthState.INFECTED && healthState != HealthState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.Instance.infectionChancePerHour)
                    {
                        healthState = HealthState.INFECTED;
                        healthStateChangeTimeStep = timeStep;
                        break;
                    }
                }
            }
        }

        if (socialState == SocialState.HOME)
        {
            foreach (Person p in familyRealtions)
            {
                if (p.socialState != SocialState.HOME)
                    continue;
                
                if ((p.healthState == HealthState.INFECTED || p.healthState == HealthState.SYMPTOMATIC) && healthState != HealthState.INFECTED && healthState != HealthState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.Instance.infectionChancePerHour)
                    {
                        healthState = HealthState.INFECTED;
                        healthStateChangeTimeStep = timeStep;
                        break;
                    }
                }
            }
        }
        
        if (socialState == SocialState.WORK)
        {
            foreach (Person p in workRelations)
            {
                if (p.socialState != SocialState.WORK)
                    continue;
                
                if ((p.healthState == HealthState.INFECTED || p.healthState == HealthState.SYMPTOMATIC) && healthState != HealthState.INFECTED && healthState != HealthState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.Instance.infectionChancePerHour)
                    {
                        healthState = HealthState.INFECTED;
                        healthStateChangeTimeStep = timeStep;
                        break;
                    }
                }
            }
        }
        
        if (socialState == SocialState.SOCIAL)
        {
            foreach (Person p in socialRelations)
            {
                if (p.socialState != SocialState.SOCIAL)
                    continue;
                
                if ((p.healthState == HealthState.INFECTED || p.healthState == HealthState.SYMPTOMATIC) && healthState != HealthState.INFECTED && healthState != HealthState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.Instance.infectionChancePerHour)
                    {
                        healthState = HealthState.INFECTED;
                        healthStateChangeTimeStep = timeStep;
                        break;
                    }
                }
            }
        }
    }

    private void UpdateSocialState(int hour)
    {
        if (healthState == HealthState.DEAD)
            return;
        
        if (hour == 8) //Wakes up
        {
            if (healthState == HealthState.SYMPTOMATIC) //Symptomatic, stays home
                socialState = SocialState.HOME;
            else //Not symptomatic, goes to work
                socialState = SocialState.WORK;
        }

        if (hour == 15)
        {
            if (healthState == HealthState.SYMPTOMATIC) //Symptomatic, stays home
                socialState = SocialState.HOME;
            else //Not symptomatic, is social
                socialState = SocialState.SOCIAL;
        }

        if (hour == 18) //Goes home
            socialState = SocialState.HOME;

        if (hour == 23) //Goes to sleep
            socialState = SocialState.SLEEPING;
    }

    public void SetRelations(SimWorld world)
    {
        Random random = new Random();
        
        byte familyRelationsCount = (byte) SimParameters.Instance.rangeOfFamilyMembers.GetRandomInRange();
        byte socialRelationsCount = (byte) SimParameters.Instance.rangeOfSocialMembers.GetRandomInRange();
        byte workRelationsCount = (byte) SimParameters.Instance.rangeOfWorkMembers.GetRandomInRange();

        while (familyRealtions.Count < familyRelationsCount)
        {
            if (familyRealtions.Count != 0)
                break;
            
            Person p = world.population[random.Next(world.population.Count)];
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
            Person p = world.population[random.Next(world.population.Count)];
            if (p == this || socialRelations.Contains(p))
                continue;
            
            socialRelations.Add(p);
            p.AddSocialMember(this);
        }
        
        while (workRelations.Count < workRelationsCount)
        {
            if (workRelations.Count != 0)
                break;
            
            Person p = world.population[random.Next(world.population.Count)];
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