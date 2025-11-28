using PopSim.World;

namespace PopSim.Policies;

public class SanitisePolicy : Policy
{
    public SanitisePolicy(string name, SimWorld world) : base(name, world)
    {
        
    }

    public override void Step()
    {
        //No effect per step
    }

    public override void EnablePolicy()
    {
        if (isEnabled)
            return;

        world.worldParameters.infectionChancePerHour *= 0.65f;
        isEnabled = true;
    }

    public override void DisablePolicy()
    {
        if (!isEnabled)
            return;

        world.worldParameters.infectionChancePerHour /= 0.65f;
        isEnabled = false;
    }
}