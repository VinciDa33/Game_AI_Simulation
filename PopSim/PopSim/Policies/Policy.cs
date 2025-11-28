using PopSim.World;

namespace PopSim.Policies;

public abstract class Policy
{
    public string name { get; private set; }
    public SimWorld world { get; private set; }
    public bool isEnabled { get; protected set; }
    
    public Policy(string name, SimWorld world)
    {
        this.name = name;
        this.world = world;
        isEnabled = false;
    }

    public abstract void Step();

    public abstract void EnablePolicy();
    public abstract void DisablePolicy();
}