using PopSim.Policies;

namespace PopSim.World;

public class PolicyManager
{
    public Policy[] policies { get; private set; }
    public List<string> policyChoices = [];
    
    public PolicyManager(SimWorld world)
    {
        policies = new Policy[]
        {
            new AwarenessPolicy("Awareness", world, this),
            new SanitisePolicy("Sanitise", world, this),
            new MaskPolicy("Mask", world, this),
            new RemotePolicy("Remote", world, this),
            new IsolationPolicy("Isolation", world, this),
            new SLockdownPolicy("Soft_Lockdown", world, this),
            new TLockdownPolicy("Total_Lockdown", world, this),
            new VaccinePolicy("Vaccine", world, this)
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

    public override string ToString()
    {
        return "Policy Choices [" + string.Join(", ", policyChoices) + "]";
    }
}