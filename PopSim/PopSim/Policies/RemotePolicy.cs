using PopSim.World;

namespace PopSim.Policies;

public class RemotePolicy : Policy
{
    public RemotePolicy(string name, SimWorld world, PolicyManager manager) : base(name, world, manager)
    {
        
    }

    public override void Step()
    {
        //No effect per step
        //Should perhaps have a slight happiness deduction
    }

    public override void EnablePolicy(int step)
    {
        if (isEnabled)
            return;

        isEnabled = true;
        manager.policyChoices.Add($"[{step}: enabled {name}]");
    }

    public override void DisablePolicy(int step)
    {
        if (!isEnabled)
            return;

        isEnabled = false;
        manager.policyChoices.Add($"[{step}: disabled {name}]");
    }
}