using PopSim.World;

namespace PopSim.Policies;

public class RemotePolicy : Policy
{
    public RemotePolicy(string name, SimWorld world) : base(name, world)
    {
        
    }

    public override void Step()
    {
        //No effect per step
        //Should perhaps have a slight happiness deduction
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