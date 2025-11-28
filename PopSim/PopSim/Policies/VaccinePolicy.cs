using PopSim.World;

namespace PopSim.Policies;

public class VaccinePolicy : Policy
{
    public VaccinePolicy(string name, SimWorld world) : base(name, world)
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