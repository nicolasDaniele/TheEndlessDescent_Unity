using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GravewardenController : AIController
{
    [Header("Gravewarden AI Controller")]
    [SerializeField]
    private string idleAnimation = "";
    [SerializeField]
    private string moveAnimation = "";
    [SerializeField]
    private string groundHitAnimation = "";
    [SerializeField]
    private string meleeAttackAnimation = "";
    [SerializeField]
    private float minIdleTime = 1f;
    [SerializeField]
    private float maxIdleTime = 5f;
    [SerializeField]
    private float minDistanceToMove = 10f;
    [SerializeField]
    private float maxDistanceToMove = 20f;
    [SerializeField] 
    // The farest distance the enemy is allowed to go from his spawninng position
    private float maxMovingDistance = 30f;

    private Vector3 spawnPosition;

    private IdleState idleState;
    private MoveState moveState;
    private AttackState meleeAttackState;
    private AttackState groundHitState;

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent.stoppingDistance = 0.1f;
        spawnPosition = transform.position;

        stateMachine.SetDefaultState(moveState);
    }

    // Sets a random distance and a random angle
    // for its destination location.
    public override void StartMoving()
    {
        float distToMove = Random.Range(minDistanceToMove, maxDistanceToMove);
        float angle = Random.Range(0f, 360f);

        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * distToMove;

        Vector3 candidatePos = transform.position + offset;

        Vector3 fromSpawn = candidatePos - spawnPosition;
        if (fromSpawn.magnitude > maxMovingDistance)
        {
            fromSpawn = fromSpawn.normalized * maxMovingDistance;
            candidatePos = spawnPosition + fromSpawn;
        }

        if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }

        base.StartMoving();
    }

    // Overrides UpdateMovement doing nothing
    // because it only moves to the destination set in StartMoving.
    public override void UpdateMovement()
    {
    }

    public override AIAction DecideNextAction(IReadOnlyCollection<AIAction> possibleActions)
    {
        // Should make a GroundHit or stay Idle?
        if(possibleActions.Contains<AIAction>(AIAction.DistanceAttack) &&
            possibleActions.Contains<AIAction>(AIAction.Idle))
        {
            return Random.Range(1, 11) > 5 ? 
                AIAction.DistanceAttack : AIAction.Idle;
        }

        // Should keep attacking?
        if(possibleActions.Contains<AIAction>(AIAction.Move) &&
            possibleActions.Contains<AIAction>(AIAction.MeleeAttack))
        {
            return IsPlayerInMeleeAttackRange && repeatsMeleeAttack ?
                AIAction.MeleeAttack : AIAction.Move;
        }

        return possibleActions.First();
    }

    protected override Dictionary<AIAction, IState> BuildActionsMap()
    {
        idleState = new IdleState(stateMachine, this, animator, idleAnimation);
        moveState = new MoveState(stateMachine, this, animator, moveAnimation);
        meleeAttackState = new AttackState(stateMachine, this, animator, meleeAttackAnimation);
        groundHitState = new AttackState(stateMachine, this, animator, groundHitAnimation);

        idleState.AddHighPriorityTransition(new PlayerInAttackRange(this, new[] { AIAction.MeleeAttack }, meleeAttackRange));
        idleState.AddLowPriorityTransition(new TimerUp(Random.Range(minIdleTime, maxIdleTime), new[] { AIAction.Move }));

        moveState.AddHighPriorityTransition(new PlayerInAttackRange(this, new[] { AIAction.MeleeAttack }, meleeAttackRange));
        moveState.AddLowPriorityTransition(new AgentReachedDestination(navMeshAgent, new[] { AIAction.Idle, AIAction.DistanceAttack }));

        meleeAttackState.AddLowPriorityTransition(new AnimationEnded(animator, new[] { AIAction.Move, AIAction.MeleeAttack }));
        groundHitState.AddLowPriorityTransition(new AnimationEnded(animator, new[] { AIAction.Move }));

        return new Dictionary<AIAction, IState> { 
            { AIAction.Move, moveState }, 
            { AIAction.Idle, idleState }, 
            { AIAction.MeleeAttack, meleeAttackState }, 
            { AIAction.DistanceAttack, groundHitState }
        };
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnPosition, maxMovingDistance);
    }
}