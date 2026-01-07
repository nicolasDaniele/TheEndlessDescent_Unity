using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum AIAction
{
    None,
    Idle,
    Move,
    MeleeAttack,
    DistanceAttack
}

/// <summary>
/// Base class responsible for decision making.
/// Chooses which AIAction to execute based on the current context
/// and the set of actions allowed by the active state transitions.
/// </summary>
public abstract class AIController : MonoBehaviour
{
    public float AnimTransitionTime => animTransitionTime;
    public bool IsPlayerInMeleeAttackRange => DistanceToTarget() < meleeAttackRange;

    [Header("AI Controller")]
    [SerializeField]
    protected float meleeAttackRange = 2f;
    [SerializeField]
    protected float distanceAttackRange = 30f;
    protected Transform target;
    [SerializeField]
    protected float animTransitionTime = 0f;
    [SerializeField]
    protected float halfFovAngle = 60f;

    // Should stay in attack state if the player is still in range?
    [SerializeField]
    protected bool repeatsMeleeAttack = true;
    [SerializeField]
    protected bool repeatsDistanceAttack = false;

    protected float cosHalfFov;

    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    protected StateMachine stateMachine;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        stateMachine = new StateMachine(this);
        stateMachine.SetActionStateMap(BuildActionsMap());

        cosHalfFov = Mathf.Cos(halfFovAngle * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Chooses the next action to execute from a set of possible actions.
    /// This method defines the "personality" of the enemy.
    /// </summary>
    /// <param name="possibleActions">
    /// Actions allowed by the current state's transition.
    /// </param>
    /// <returns>
    /// The chosen AIAction.
    /// </returns>
    public virtual AIAction DecideNextAction(IReadOnlyCollection<AIAction> possibleActions)
    {
        return possibleActions.First();
    }

    /// <summary>
    /// Creates a map with the enemy's states and the actions related to them.
    /// That map should be passed to the StateMachine to manage the corresponding states.
    /// </summary>
    /// <returns>
    /// The enemy's states and related AIActions.
    /// </returns>
    protected virtual Dictionary<AIAction, IState> BuildActionsMap()
    {
        return new Dictionary<AIAction, IState>();
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public virtual void StartMoving()
    {
        navMeshAgent.isStopped = false;
    }

    public virtual void UpdateMovement()
    {
        navMeshAgent.SetDestination(target.position);
    }

    public virtual void StopMoving()
    {
        navMeshAgent.isStopped = true;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public bool IsTargetInFront()
    {
        if(target == null)
            return false;

        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToTarget);
        
        return dot >= cosHalfFov;
    }
}
