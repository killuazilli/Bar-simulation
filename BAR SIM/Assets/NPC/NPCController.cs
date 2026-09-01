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

    [Header("Interaction")]
    [SerializeField] private BartenderInteraction bartenderInteraction;

    private bool hasArrivedAtBar;
    private bool isLeaving;

    // Gives other scripts access to this NPC's data
    public NPCData NPCData => npcData;

    // Gets the NPC name from NPCData
    public string NPCName => npcData != null ? npcData.npcName : gameObject.name;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        WalkToBar();
    }

    private void Update()
    {
        if (agent == null || agent.pathPending)
            return;

        // Check if NPC has reached the bar
        if (!hasArrivedAtBar &&
            agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            ArriveAtBar();
        }

        // Check if NPC has reached the exit
        if (isLeaving &&
            agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            FinishLeaving();
        }
    }

    private void WalkToBar()
    {
        if (barServicePoint == null)
        {
            Debug.LogError(NPCName + " has no Bar Service Point assigned.");
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(barServicePoint.position);
    }

    private void ArriveAtBar()
    {
        hasArrivedAtBar = true;

        agent.isStopped = true;
        agent.ResetPath();

        FaceBartender();

        if (bartenderInteraction != null)
        {
            bartenderInteraction.NPCArrived(this);
        }
        else
        {
            Debug.LogError(NPCName + " has no BartenderInteraction assigned.");
        }
    }

    private void FaceBartender()
    {
        if (bartenderInteraction == null)
            return;

        Vector3 direction =
            bartenderInteraction.transform.position - transform.position;

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
            Debug.LogError(NPCName + " has no Exit Point assigned.");
            return;
        }

        isLeaving = true;

        agent.isStopped = false;
        agent.SetDestination(exitPoint.position);
    }

    private void FinishLeaving()
    {
        isLeaving = false;

        agent.ResetPath();

        gameObject.SetActive(false);
    }
}