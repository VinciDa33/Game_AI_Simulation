using PopSim.States;

namespace PopSim.Utility;

public class ScheduleItem
{
    public SocialState socialState;
    public IntRange timeSpan;

    public ScheduleItem(SocialState socialState, IntRange timeSpan)
    {
        this.socialState = socialState;
        this.timeSpan = timeSpan;
    }
}