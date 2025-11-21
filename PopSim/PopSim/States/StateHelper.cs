namespace PopSim.States;

public static class StateHelper
{
    public static bool IsInfectious(HealthState state)
    {
        return state == HealthState.INFECTED || state == HealthState.SYMPTOMATIC;
    }
}