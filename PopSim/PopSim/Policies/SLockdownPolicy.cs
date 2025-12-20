using PopSim.World;

namespace PopSim.Policies;

public class SLockdownPolicy : Policy
{
    public SLockdownPolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
    {
        
    }

    public override void Step()
    {
        if (isEnabled)
            world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.005f); //In a population of 1000, the starting happiness will be 5000, and this number will be 5 (per hour)
    }

    public override void EnablePolicy(int step)
    {
        if (isEnabled)
            return;

        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }

    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;

        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}