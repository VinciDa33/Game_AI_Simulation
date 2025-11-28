using PopSim.World;

namespace PopSim.Policies;

public class IsolationPolicy : Policy
{
    public IsolationPolicy(string name, SimWorld world) : base(name, world)
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

        world.happiness -= 5;

        isEnabled = true;
    }

    public override void DisablePolicy()
    {
        if (!isEnabled)
            return;

        world.happiness += 3;
        
        isEnabled = false;
    }
}