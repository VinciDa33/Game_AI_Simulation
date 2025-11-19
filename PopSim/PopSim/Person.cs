namespace PopSim;

public class Person
{
    public PersonState state { get; private set; }= PersonState.HEALTHY;
    public SocialState SocialState { get; private set; }  = SocialState.HOME;
    private byte timeSinceStateChange = 0;
    
    public List<Person> familyRealtions { get; private set; } = new List<Person>();
    public List<Person> socialRelations { get; private set; } = new List<Person>();
    public List<Person> workRelations { get; private set; } = new List<Person>();

    public void Update(int hour)
    {
        if (state == PersonState.DEAD)
            return;
        
        UpdateSocialState(hour);
        UpdateInfectedState();
        UpdateHealth();
        
    }

    private void UpdateHealth()
    {
        Random random = new Random();
        timeSinceStateChange++;

        if (state == PersonState.INFECTED)
        {
            if (random.NextDouble() < SimParameters.chanceOfRecoveryFromInfectionPerHour)
            {
                state = PersonState.RECOVERED;
                timeSinceStateChange = 0;
            }
        }

        if (state == PersonState.INFECTED)
        {
            if (timeSinceStateChange >= SimParameters.meanTimeFromInfectionToSymptomatic)
            {
                state = PersonState.SYMPTOMATIC;
                timeSinceStateChange = 0;
            }
        }

        if (state == PersonState.SYMPTOMATIC)
        {
            if (random.NextDouble() < SimParameters.chanceOfRecoveryFromSymptomaticPerHour)
            {
                state = PersonState.RECOVERED;
                timeSinceStateChange = 0;
                
            }
        }

        if (state == PersonState.SYMPTOMATIC)
        {
            if (timeSinceStateChange >= SimParameters.meanTimeFromSymptomaticToDeath)
            {
                state = PersonState.DEAD;
                timeSinceStateChange = 0;
            }
        }
    }
    
    private void UpdateInfectedState()
    {
        Random random = new Random();
        
        if (state == PersonState.RECOVERED && SimParameters.immuneWithAntibodies)
            return;
        
        if (SocialState == SocialState.SLEEPING)
        {
            foreach (Person p in familyRealtions)
            {
                if ((p.state == PersonState.INFECTED || p.state == PersonState.SYMPTOMATIC) && state != PersonState.INFECTED && state != PersonState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.infectionChancePerHour)
                    {
                        state = PersonState.INFECTED;
                        timeSinceStateChange = 0;
                        break;
                    }
                }
            }
        }

        if (SocialState == SocialState.HOME)
        {
            foreach (Person p in familyRealtions)
            {
                if (p.SocialState != SocialState.HOME)
                    continue;
                
                if ((p.state == PersonState.INFECTED || p.state == PersonState.SYMPTOMATIC) && state != PersonState.INFECTED && state != PersonState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.infectionChancePerHour)
                    {
                        state = PersonState.INFECTED;
                        timeSinceStateChange = 0;
                        break;
                    }
                }
            }
        }
        
        if (SocialState == SocialState.WORK)
        {
            foreach (Person p in workRelations)
            {
                if (p.SocialState != SocialState.WORK)
                    continue;
                
                if ((p.state == PersonState.INFECTED || p.state == PersonState.SYMPTOMATIC) && state != PersonState.INFECTED && state != PersonState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.infectionChancePerHour)
                    {
                        state = PersonState.INFECTED;
                        timeSinceStateChange = 0;
                        break;
                    }
                }
            }
        }
        
        if (SocialState == SocialState.SOCIAL)
        {
            foreach (Person p in socialRelations)
            {
                if (p.SocialState != SocialState.SOCIAL)
                    continue;
                
                if ((p.state == PersonState.INFECTED || p.state == PersonState.SYMPTOMATIC) && state != PersonState.INFECTED && state != PersonState.SYMPTOMATIC)
                {
                    if (random.NextDouble() < SimParameters.infectionChancePerHour)
                    {
                        state = PersonState.INFECTED;
                        timeSinceStateChange = 0;
                        break;
                    }
                }
            }
        }
    }

    private void UpdateSocialState(int hour)
    {
        if (state == PersonState.DEAD)
            return;
        
        if (hour == 8) //Wakes up
        {
            if (state == PersonState.SYMPTOMATIC) //Symptomatic, stays home
                SocialState = SocialState.HOME;
            else //Not symptomatic, goes to work
                SocialState = SocialState.WORK;
        }

        if (hour == 15)
        {
            if (state == PersonState.SYMPTOMATIC) //Symptomatic, stays home
                SocialState = SocialState.HOME;
            else //Not symptomatic, is social
                SocialState = SocialState.SOCIAL;
        }

        if (hour == 18) //Goes home
            SocialState = SocialState.HOME;

        if (hour == 23) //Goes to sleep
            SocialState = SocialState.SLEEPING;
    }

    public void SetRelations()
    {
        Random random = new Random();
        
        byte familyRelationsCount = (byte) SimParameters.rangeOfFamilyMembers.GetRandomInRange();
        byte socialRelationsCount = (byte) SimParameters.rangeOfSocialMembers.GetRandomInRange();
        byte workRelationsCount = (byte) SimParameters.rangeOfWorkMembers.GetRandomInRange();

        while (familyRealtions.Count < familyRelationsCount)
        {
            if (familyRealtions.Count != 0)
                break;
            
            Person p = World.Instance.population[random.Next(World.Instance.population.Count)];
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
            Person p = World.Instance.population[random.Next(World.Instance.population.Count)];
            if (p == this || socialRelations.Contains(p))
                continue;
            
            socialRelations.Add(p);
            p.AddSocialMember(this);
        }
        
        while (workRelations.Count < workRelationsCount)
        {
            if (workRelations.Count != 0)
                break;
            
            Person p = World.Instance.population[random.Next(World.Instance.population.Count)];
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
        state = PersonState.INFECTED;
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