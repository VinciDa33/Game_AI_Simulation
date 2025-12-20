using PopSim.World;

namespace PopSim.Policies;

public abstract class Policy
{
    public string name { get; private set; }
    public SimWorld world { get; private set; }
    public PolicyManager manager;
    public bool isEnabled { get; protected set; }
    
    public Policy(string name, SimWorld world, PolicyManager manager)
    {
        this.name = name;
        this.world = world;
        this.manager = manager;
        isEnabled = false;
    }

    public abstract void Step();

    public abstract void EnablePolicy(int step);
    public abstract void DisablePolicy(int step);
}