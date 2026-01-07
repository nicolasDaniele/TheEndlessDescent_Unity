using System.Collections.Generic;
using System.Linq;

public class PlayerInAttackRange : ITransition
{
    public IReadOnlyCollection<AIAction> PossibleNextStates { get; }

    private AIController controller;
    private float attackRange;
    private bool invertCondition;

    public PlayerInAttackRange(AIController _controller, IEnumerable<AIAction> _actions, 
        float _attackRange, bool _invertCondition = false)
    {
        controller = _controller;
        PossibleNextStates = _actions.ToList<AIAction>();
        attackRange = _attackRange;
        invertCondition = _invertCondition;
    }

    public bool ShouldTransition()
    {
        bool inRange = controller.DistanceToTarget() <= attackRange;
        bool inFront = controller.IsTargetInFront();
        bool conditionMet = inRange && inFront;

        return invertCondition ? !conditionMet : conditionMet;
    }
}
