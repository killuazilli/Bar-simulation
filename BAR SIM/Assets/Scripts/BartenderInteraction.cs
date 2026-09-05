using UnityEngine;
using TMPro;

public class BartenderInteraction : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private CameraManager cameraManager;

    [Header("Interaction UI")]
    [SerializeField]
    private GameObject interactionPrompt;

    [SerializeField]
    private TMP_Text interactionText;

    private NPCController currentNPC;

    private bool canInteract;
    private bool interactionStarted;

    public NPCController CurrentNPC =>
        currentNPC;

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (!canInteract)
            return;

        if (interactionStarted)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartNPCInteraction();
        }
    }

    public void NPCArrived(
        NPCController npc)
    {
        if (npc == null)
            return;

        currentNPC = npc;

        canInteract = true;
        interactionStarted = false;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }

        if (interactionText != null)
        {
            interactionText.text =
                "Press E to talk to " +
                npc.NPCName;
        }

        Debug.Log(
            npc.NPCName +
            " has arrived at the bar."
        );
    }

    private void StartNPCInteraction()
    {
        if (currentNPC == null)
            return;

        canInteract = false;
        interactionStarted = true;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // Switch to Camera 2.
        if (cameraManager != null)
        {
            cameraManager.ShowInteractionCamera();
        }

        // Start Ink conversation.
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(
                currentNPC
            );
        }
    }

    public void EndNPCInteraction()
    {
        canInteract = false;
        interactionStarted = false;

        currentNPC = null;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}