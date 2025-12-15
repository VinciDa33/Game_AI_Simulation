using PopSim.Sorting;
using PopSim.Utility;

namespace PopSim.Genetic_Algorithm;

public static class NSGAII
{
    /// <summary>
    /// Generates a list of Pareto fronts based on domination
    /// </summary>
    /// <param name="agents"></param>
    /// <returns></returns>
    public static ParetoFronts GenerateParetoFronts(List<Agent> agents)
    {
        Console.WriteLine("- Generating Pareto Fronts based on domination");
        
        List<ParetoSubject> subjects = new List<ParetoSubject>();
        foreach (Agent agent in agents)
            subjects.Add(new ParetoSubject(agent));
        
        ParetoFronts fronts = new ParetoFronts();

        //We keep going until all subjects have gotten a pareto front ranking
        while (subjects.Count > 0)
        {
            //Reset the domination count for the next round
            foreach (ParetoSubject subject in subjects)
                subject.dominatedCount = 0;
            
            //Iterate over all agents
            foreach (ParetoSubject subject in subjects)
            {
                //Compare to all other agents
                foreach (ParetoSubject otherSubject in subjects)
                {
                    //Remember we are maximizing happiness and minimizing death count. If this combined statement is true, 'subject' is dominating 'otherSubject'
                    if (subject.agent.GetFitnessHappiness() >= otherSubject.agent.GetFitnessHappiness() &&
                        subject.agent.GetFitnessDeathCount() <= otherSubject.agent.GetFitnessDeathCount() &&
                        (subject.agent.GetFitnessHappiness() > otherSubject.agent.GetFitnessHappiness() || subject.agent.GetFitnessDeathCount() < otherSubject.agent.GetFitnessDeathCount()))
                    {
                        otherSubject.dominatedCount++;
                    } 
                }
            }

            //Find all non-dominated subjects
            List<ParetoSubject> nonDominatedSubjects = [];
            foreach (ParetoSubject subject in subjects)
            {
                //If a subject was not dominated at all, it will be part of the next front
                if (subject.dominatedCount == 0)
                    nonDominatedSubjects.Add(subject);
            }
            
            List<Agent> nextFront = [];
            //Remove all subjects that were non-dominated this round, and add them to the next front
            foreach (ParetoSubject subject in nonDominatedSubjects)
            {
                subjects.Remove(subject);
                nextFront.Add(subject.agent);
            }
            
            //Set the next front on the ParetoFronts object
            Console.WriteLine($"Pareto front {fronts.fronts.Count + 1} finished with {nextFront.Count} agents");
            fronts.AddFront(nextFront);
            
            //If there are still subjects left in the subjects list, this process will reset and repeat
            //continuously removing the best subjects, until all agents have received a front rank.
            
        }

        Console.WriteLine("- :> Generated " + fronts.fronts.Count + " Pareto fronts\n");
        return fronts;
    }
    
    
// #########################################################################################################################################################################
// #########################################################################################################################################################################

    public static Dictionary<Agent, double> CrowdingAllFronts(ParetoFronts paretoFronts)
    {
        Console.WriteLine("- Calculating crowding distances");
        
        Dictionary<Agent, double> crowdingDistances = new Dictionary<Agent, double>();

        //Loop through all Pareto fronts
        foreach (List<Agent> front in paretoFronts.fronts)
        {
            //Get crowding distances for all agents in a single front
            Dictionary<Agent, double> frontCrowdingDistances = NSGAII.Crowding(front);
            
            //Add all crowding distances to the final dictionary
            foreach (var (key, value) in frontCrowdingDistances)
                crowdingDistances.TryAdd(key, value);
        }
        
        Console.WriteLine("- :> Finished calculating crowding distances\n");

        return crowdingDistances;
    }

    
// #########################################################################################################################################################################
// #########################################################################################################################################################################


    /// <summary>
    /// Calculates the crowding distances for a single Pareto front
    /// </summary>
    /// <param name="paretoFront"></param>
    /// <returns></returns>
    public static Dictionary<Agent, double> Crowding(List<Agent> paretoFront)
    {
        //Create and fill out dictionary with agent keys
        Dictionary<Agent, double> crowdingDistances = new Dictionary<Agent, double>();
        foreach (Agent agent in paretoFront)
            crowdingDistances.TryAdd(agent, 0d);

        //If the front contains less than 3 agents, they all get infinite crowding distance
        if (paretoFront.Count < 3)
        {
            foreach (Agent agent in paretoFront)
                crowdingDistances[agent] += double.PositiveInfinity;
            
            return crowdingDistances;
        }

        //Sort all agents by their death fitness
        paretoFront.Sort(new DeathComparator());

        //Get the total range size of death fitness
        double min = paretoFront[0].GetFitnessDeathCount(); //This should be the lowest death fitness value in this front
        double max = paretoFront[^1].GetFitnessDeathCount(); //This should be the highest death fitness value in this front
        double range = max - min;
        
        //Assign infinite values to the edges
        crowdingDistances[paretoFront[0]] += double.PositiveInfinity;
        crowdingDistances[paretoFront[^1]] += double.PositiveInfinity;

        for (int i = 1; i < paretoFront.Count - 1; i++)
        {
            //Get the fitness values of the two neighbouring agents
            double nextValue = paretoFront[i + 1].GetFitnessDeathCount();
            double previousValue = paretoFront[i - 1].GetFitnessDeathCount();
            
            //Normalizing contribution using range
            double contribution = range != 0 ? (nextValue - previousValue) / range : 0d;
            crowdingDistances[paretoFront[i]] += contribution;
        }
        
        
        
        //Sort all agents by their happiness fitness
        paretoFront.Sort(new HappinessComparator());

        //Get the total range size of happiness fitness
        min = paretoFront[0].GetFitnessHappiness(); //This should be the lowest happiness fitness value in this front
        max = paretoFront[^1].GetFitnessHappiness(); //This should be the highest happiness fitness value in this front
        range = max - min;
        
        //Assign infinite values to the edges (they are now sorted by happiness instead of death)
        crowdingDistances[paretoFront[0]] += double.PositiveInfinity;
        crowdingDistances[paretoFront[^1]] += double.PositiveInfinity;
        
        for (int i = 1; i < paretoFront.Count - 1; i++)
        {
            //Get the fitness values of the two neighbouring agents
            double nextValue = paretoFront[i + 1].GetFitnessHappiness();
            double previousValue = paretoFront[i - 1].GetFitnessHappiness();
            
            //Normalizing contribution using range
            double contribution = range != 0 ? (nextValue - previousValue) / range : 0d;
            crowdingDistances[paretoFront[i]] += contribution;
        }
        
        return crowdingDistances;
    }

    
// #########################################################################################################################################################################
// #########################################################################################################################################################################
    
    
    /// <summary>
    /// Truncates the population down to a desired size, based on Pareto front ranks and crowding distances
    /// </summary>
    /// <param name="agents"></param>
    /// <param name="desiredSize"></param>
    public static List<Agent> DeterministicTruncation(ParetoFronts paretoFronts, Dictionary<Agent, double> crowdingDistances, int desiredSize)
    {
        Console.WriteLine("- Truncating population to size " + desiredSize);
        
        List<Agent> newPopulation = [];
        
        //Go through each front from best to worst
        foreach (List<Agent> front in paretoFronts.fronts)
        {
            //If there is space for the current front add them all to the new population
            if (newPopulation.Count + front.Count <= desiredSize)
                newPopulation.AddRange(front);
            
            //Otherwise, we will need to look at crowding distance
            else
            {
                //Calculate the remaining space
                int remainingSpace = desiredSize - newPopulation.Count;
                
                //Sort the front based on crowding distance
                front.Sort(new CrowdingComparator(crowdingDistances));
                
                //Fill out the new population
                for (int i = 0; i < remainingSpace; i++)
                    newPopulation.Add(front[i]);
            }
        }

        Console.WriteLine("- :> Finished truncating population to size " + newPopulation.Count + "\n");
        
        return newPopulation;
    }
    
     
// #########################################################################################################################################################################
// #########################################################################################################################################################################

    
    
    /// <summary>
    /// Select the best of two random agents
    /// </summary>
    /// <param name="agents"></param>
    /// <returns></returns>
    public static Agent TournamentSelection(List<Agent> agents, ParetoFronts paretoFronts, Dictionary<Agent, double> crowdingDistances)
    {
        //Pick two random contestants from the list of agents
        Agent contestantA = agents[RandomManager.Instance.GetNextInt(agents.Count)];
        Agent contestantB = agents[RandomManager.Instance.GetNextInt(agents.Count)];

        //Compare front ranks
        if (paretoFronts.GetFrontRank(contestantA) < paretoFronts.GetFrontRank(contestantB))
            return contestantA;
        if (paretoFronts.GetFrontRank(contestantA) > paretoFronts.GetFrontRank(contestantB))
            return contestantB;
        
        //Compare crowding distances
        if (crowdingDistances[contestantA] >= crowdingDistances[contestantB])
            return contestantA;
        return contestantB;
    }
}