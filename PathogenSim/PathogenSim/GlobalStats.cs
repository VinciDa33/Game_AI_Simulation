namespace PathogenSim;

public static class GlobalStats
{
    public static float FatalityRate { get; private set; } = 0;
    public static float InfectionRate { get; private set; } = 0;
    
    public static float PublicApproval { get; private set; } = 100;
}