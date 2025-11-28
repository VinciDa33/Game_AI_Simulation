using PopSim.World;

namespace PopSim.Policies;

public class SLockdownPolicy : Policy
{
    public SLockdownPolicy(string name, SimWorld world) : base(name, world)
    {
        
    }

    public override void Step()
    {
        if (isEnabled)
            world.happiness -= 2;
    }

    public override void EnablePolicy()
    {
        if (isEnabled)
            return;

        isEnabled = true;
    }

    public override void DisablePolicy()
    {
        if (!isEnabled)
            return;

        isEnabled = false;
    }
}