using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class AgentReachedDestination : ITransition
{
    public IReadOnlyCollection<AIAction> PossibleNextStates { get; }

    private NavMeshAgent agent;

    public AgentReachedDestination(NavMeshAgent _agent, IEnumerable<AIAction> _actions)
    {
        agent = _agent;
        PossibleNextStates = _actions.ToList<AIAction>();
    }

    public bool ShouldTransition()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude > 0.01f)
            return false;

        return true;
    }
}