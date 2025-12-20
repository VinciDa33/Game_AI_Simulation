using PopSim.World;

namespace PopSim.Policies;

public class IsolationPolicy : Policy
{
    public IsolationPolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
    {
        
    }

    public override void Step()
    {
        //No effect per step
    }

    public override void EnablePolicy(int step)
    {
        if (isEnabled)
            return;

        world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.25f); //In a population of 1000, the starting happiness will be 5000, and this number will be 250

        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }

    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;

        world.worldState.happiness += (int) Math.Floor(world.population.Count * 0.2f); //In a population of 1000, the starting happiness will be 5000, and this number will be 200
        
        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}