using PopSim.Policies;

namespace PopSim.World;

public class PolicyManager
{
    public Policy[] policies { get; private set; }
    
    public PolicyManager(SimWorld world)
    {
        policies = new Policy[]
        {
            new AwarenessPolicy("Awareness", world),
            new SanitisePolicy("Sanitise", world),
            new MaskPolicy("Mask", world),
            new RemotePolicy("Remote", world),
            new IsolationPolicy("Isolation", world),
            new SLockdownPolicy("Soft_Lockdown", world),
            new TLockdownPolicy("Total_Lockdown", world),
            new VaccinePolicy("Vaccine", world)
        };
    }

    public void StepPolicies()
    {
        foreach (Policy policy in policies)
        {
            policy.Step();
        }
    }
    
    public Policy? GetPolicy(string name)
    {
        foreach (Policy policy in policies)
        {
            if (policy.name.Equals(name))
                return policy;
        }

        return null;
    }
}