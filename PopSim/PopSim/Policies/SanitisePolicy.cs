using PopSim.World;

namespace PopSim.Policies;

public class SanitisePolicy : Policy
{
    public SanitisePolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
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

        world.worldParameters.infectionChancePerHour *= 0.65f;
        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }

    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;

        world.worldParameters.infectionChancePerHour /= 0.65f;
        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}