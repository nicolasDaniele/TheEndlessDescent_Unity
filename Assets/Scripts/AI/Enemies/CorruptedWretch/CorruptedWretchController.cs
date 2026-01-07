using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorruptedWretchController : AIController
{
    [Header("Corrupted Wretch AI Controller")]
    [SerializeField]
    private string moveAnimation = "";
    [SerializeField]
    private string attackAnimation = "";

    private MoveState moveState;
    private AttackState meleeAttackState;

    protected override void Awake()
    {
        base.Awake();

        stateMachine.SetDefaultState(moveState);
    }

    protected override Dictionary<AIAction, IState> BuildActionsMap()
    {
        moveState = new MoveState(stateMachine, this, animator, moveAnimation);
        meleeAttackState = new AttackState(stateMachine, this, animator, attackAnimation, true);

        moveState.AddHighPriorityTransition(new PlayerInAttackRange(this, new[] { AIAction.MeleeAttack }, meleeAttackRange));
        meleeAttackState.AddLowPriorityTransition(new AnimationEnded(animator, new[] { AIAction.Move, AIAction.MeleeAttack }));

        return new Dictionary<AIAction, IState> {
            { AIAction.Move, moveState },
            { AIAction.MeleeAttack, meleeAttackState },
        };
    }

    public override AIAction DecideNextAction(IReadOnlyCollection<AIAction> possibleActions)
    {
        // Should keep attacking?
        if (possibleActions.Contains<AIAction>(AIAction.Move) &&
            possibleActions.Contains<AIAction>(AIAction.MeleeAttack))
        {
            return IsPlayerInMeleeAttackRange && repeatsMeleeAttack ?
                AIAction.MeleeAttack : AIAction.Move;
        }

        return possibleActions.First();
    }
}