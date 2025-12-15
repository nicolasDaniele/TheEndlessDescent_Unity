using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;

public class CharacterMoveToTarget : MonoBehaviour
{
    [Header("NavMeshAgent")]
    [SerializeField]
    private LayerMask clickableLayer;
    [SerializeField]
    private float lookRotationSpeed = 5f;
    [SerializeField]
    private float navMeshSampleDistance = 0.5f;

    [Header("Animation")]
    [SerializeField]
    private string moveAnimationName = "Run";
    [SerializeField]
    private string idleAnimationName = "Idle";
    [SerializeField]
    private float transitionDuration = 0f;

    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        navMeshAgent.speed = PlayerCharacterDatabaseAccessor.GetPlayerCharacterStats().Speed;
    }

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerMobileMapping.Enable();
        playerInputActions.PlayerMobileMapping.SetTargetLocation.performed += MoveToClickPosition;
    }

    private void OnDisable()
    {
        playerInputActions.PlayerMobileMapping.SetTargetLocation.performed -= MoveToClickPosition;
        playerInputActions.PlayerMobileMapping.Disable();
    }

    private void MoveToClickPosition(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, clickableLayer))
        {
            if(NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(navMeshHit.position);

                StartCoroutine(FaceTarget());
                StartCoroutine(UpdateAnimation());
            }
        }
    }

    private IEnumerator FaceTarget()
    {
        Vector3 direction = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        Quaternion startRot = transform.rotation;

        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * lookRotationSpeed;
            transform.rotation = Quaternion.Lerp(startRot, lookRotation, t);

            if (t >= 1f)
                break;

            yield return null;
        }

        transform.rotation = lookRotation;
    }

    private IEnumerator UpdateAnimation()
    {
        while (navMeshAgent.pathPending ||
            navMeshAgent.velocity.sqrMagnitude > 0.01f ||
            navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            animator?.CrossFade(moveAnimationName, transitionDuration);
            yield return null;
        }

        animator?.CrossFade(idleAnimationName, transitionDuration);
    }
}