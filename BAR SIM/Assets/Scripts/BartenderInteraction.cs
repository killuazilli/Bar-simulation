using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

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

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartNPCInteraction();
        }
    }

    public void NPCArrived(NPCController npc)
    {
        Debug.Log("BartenderInteraction received NPC: " + npc.NPCName);

        if (npc == null)
            return;

        currentNPC = npc;

        canInteract = true;
        interactionStarted = false;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);

            Debug.Log("Interaction Prompt ENABLED");
        }
        else
        {
            Debug.LogError("Interaction Prompt is NOT assigned!");
        }

        if (interactionText != null)
        {
            interactionText.text =
                "Press E to talk to " + npc.NPCName;
        }
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

        // Switch to interaction camera.
        if (cameraManager != null)
        {
            cameraManager.ShowInteractionCamera();
        }

        // Start Ink dialogue.
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