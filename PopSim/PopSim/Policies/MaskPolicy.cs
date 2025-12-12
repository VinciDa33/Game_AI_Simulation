using PopSim.World;

namespace PopSim.Policies;

public class MaskPolicy : Policy
{
    public MaskPolicy(string name, SimWorld world) : base(name, world)
    {
        
    }

    public override void Step()
    {
        //Slight unhappiness with wearing masks
        if (isEnabled)
            world.worldState.happiness -= 2;
    }

    public override void EnablePolicy()
    {
        if (isEnabled)
            return;

        world.worldState.happiness -= 10;
        
        world.worldParameters.infectionChancePerHour *= 0.5f;
        isEnabled = true;
    }

    public override void DisablePolicy()
    {
        if (!isEnabled)
            return;

        world.worldState.happiness += 6;
        
        world.worldParameters.infectionChancePerHour /= 0.5f;
        isEnabled = false;
    }
}