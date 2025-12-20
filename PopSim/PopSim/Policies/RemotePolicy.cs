using PopSim.World;

namespace PopSim.Policies;

public class RemotePolicy : Policy
{
    public RemotePolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
    {
        
    }

    public override void Step()
    {
        //No effect per step
        //Should perhaps have a slight happiness deduction
        world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.005f); //In a population of 1000, the starting happiness will be 5000, and this number will be 5 (per hour)
    }

    public override void EnablePolicy(int step)
    {
        if (isEnabled)
            return;
        
        world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.2f); //In a population of 1000, the starting happiness will be 5000, and this number will be 200

        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }

    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;
        
        world.worldState.happiness += (int) Math.Floor(world.population.Count * 0.15f); //In a population of 1000, the starting happiness will be 5000, and this number will be 150

        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}