using PopSim.Genetic_Algorithm;

namespace PopSim.Sim;

public class LogSim
{
    private Agent agent;

    public LogSim(Agent agent)
    {
        this.agent = agent;
    }

    public void Start()
    {
        agent.Run();
        
        //TODO: DO EXTENSIVE LOGGING HERE!
    }
}