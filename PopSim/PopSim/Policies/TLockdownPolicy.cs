using PopSim.World;

namespace PopSim.Policies;

public class TLockdownPolicy : Policy
{
    public TLockdownPolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
    {
        
    }

    public override void Step()
    {
        if (isEnabled)
            world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.01f); //In a population of 1000, the starting happiness will be 5000, and this number will be 10 (per hour)
    }

    public override void EnablePolicy(int step)
    {
        if (isEnabled)
            return;

        world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.45f); //In a population of 1000, the starting happiness will be 5000, and this number will be 450
        
        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }
    
    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;

        world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.3f); //In a population of 1000, the starting happiness will be 5000, and this number will be 300

        
        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}