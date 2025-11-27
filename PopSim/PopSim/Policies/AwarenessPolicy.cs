using PopSim.World;

namespace PopSim.Policies;

public class AwarenessPolicy : Policy
{
    public AwarenessPolicy(string name, SimWorld world) : base(name, world)
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

        world.worldParameters.infectionChancePerHour *= 0.85f;
        isEnabled = true;
    }

    public override void DisablePolicy()
    {
        if (!isEnabled)
            return;

        world.worldParameters.infectionChancePerHour /= 0.85f;
        isEnabled = false;
    }
}