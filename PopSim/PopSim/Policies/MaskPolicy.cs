using PopSim.World;

namespace PopSim.Policies;

public class MaskPolicy : Policy
{
    public MaskPolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
    {
        
    }

    public override void Step()
    {
        //Slight unhappiness with wearing masks
        if (isEnabled)
            world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.005f); //In a population of 1000, the starting happiness will be 5000, and this number will be 5 (per hour)
    }

    public override void EnablePolicy(int step)
    {
        if (isEnabled)
            return;

        world.worldState.happiness -= (int) Math.Floor(world.population.Count * 0.25f); //In a population of 1000, the starting happiness will be 5000, and this number will be 250
        
        world.worldParameters.infectionChancePerHour *= 0.5f;
        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }

    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;

        world.worldState.happiness += (int) Math.Floor(world.population.Count * 0.22f); //In a population of 1000, the starting happiness will be 5000, and this number will be 200
        
        world.worldParameters.infectionChancePerHour /= 0.5f;
        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}