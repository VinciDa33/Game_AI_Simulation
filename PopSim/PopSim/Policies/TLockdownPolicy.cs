using PopSim.World;

namespace PopSim.Policies;

public class TLockdownPolicy : Policy
{
    public TLockdownPolicy(string name, SimWorld world) : base(name, world)
    {
        
    }

    public override void Step()
    {
        if (isEnabled)
            world.worldState.happiness -= 10;
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