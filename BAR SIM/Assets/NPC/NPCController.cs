using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private NPCData npcData;

    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform barServicePoint;
    [SerializeField] private Transform exitPoint;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Interaction")]
    [SerializeField] private BartenderInteraction bartenderInteraction;

    [Header("Game")]
    [SerializeField] private GameManager gameManager;

    private bool isWalkingToBar;
    private bool hasArrivedAtBar;
    private bool isLeaving;

    public NPCData NPCData => npcData;

    public string NPCName =>
        npcData != null
            ? npcData.npcName
            : gameObject.name;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        UpdateAnimation();

        if (agent == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (isWalkingToBar &&
            HasReachedDestination())
        {
            ArriveAtBar();
            return;
        }

        if (isLeaving &&
            HasReachedDestination())
        {
            FinishLeaving();
        }
    }

    public void BeginVisit()
    {
        hasArrivedAtBar = false;
        isLeaving = false;

        WalkToBar();
    }

    private void WalkToBar()
    {
        if (barServicePoint == null)
        {
            Debug.LogError(
                NPCName +
                " has no BarServicePoint assigned."
            );

            return;
        }

        if (agent == null ||
            !agent.isOnNavMesh)
        {
            Debug.LogError(
                NPCName +
                " is not positioned on a NavMesh."
            );

            return;
        }

        isWalkingToBar = true;
        isLeaving = false;

        agent.isStopped = false;

        agent.SetDestination(
            barServicePoint.position
        );
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance >
            agent.stoppingDistance)
        {
            return false;
        }

        if (agent.hasPath &&
            agent.velocity.sqrMagnitude > 0.01f)
        {
            return false;
        }

        return true;
    }

    private void ArriveAtBar()
    {
        isWalkingToBar = false;
        hasArrivedAtBar = true;

        agent.isStopped = true;
        agent.ResetPath();

        SetWalking(false);

        FaceBartender();

        if (bartenderInteraction != null)
        {
            bartenderInteraction.NPCArrived(this);
        }
    }

    private void FaceBartender()
    {
        if (bartenderInteraction == null)
            return;

        Vector3 direction =
            bartenderInteraction.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    public void LeaveBar()
    {
        if (exitPoint == null)
        {
            Debug.LogError(
                NPCName +
                " has no ExitPoint assigned."
            );

            return;
        }

        if (agent == null ||
            !agent.isOnNavMesh)
        {
            return;
        }

        hasArrivedAtBar = false;
        isWalkingToBar = false;
        isLeaving = true;

        agent.isStopped = false;

        agent.SetDestination(
            exitPoint.position
        );
    }

    private void FinishLeaving()
    {
        isLeaving = false;

        agent.ResetPath();

        SetWalking(false);

        if (gameManager != null)
        {
            gameManager.CustomerExited(this);
        }
    }

    private void UpdateAnimation()
    {
        if (agent == null ||
            animator == null)
        {
            return;
        }

        bool walking =
            !agent.isStopped &&
            agent.velocity.sqrMagnitude > 0.01f;

        SetWalking(walking);
    }

    private void SetWalking(bool walking)
    {
        if (animator != null)
        {
            animator.SetBool(
                "IsWalking",
                walking
            );
        }
    }
}