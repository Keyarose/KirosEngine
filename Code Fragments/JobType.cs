public class JobType
{
    private string _jobName;
    
    //collection of actions that can be performed by this job type
    private Dictionary<string, JobAction> _actions;
    
    private List<JobKnowledge> _requirements;
}