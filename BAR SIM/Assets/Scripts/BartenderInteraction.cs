using UnityEngine;

public class BartenderInteraction : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private NPCController currentNPC;

    public NPCController CurrentNPC => currentNPC;

    public void NPCArrived(NPCController npc)
    {
        if (npc == null || currentNPC != null)
            return;

        currentNPC = npc;

        Debug.Log(npc.NPCName + " has arrived at the bar.");

        StartNPCInteraction();
    }

    private void StartNPCInteraction()
    {
        if (currentNPC == null)
            return;

        dialogueManager.StartDialogue(currentNPC);
    }

    public void EndNPCInteraction()
    {
        currentNPC = null;
    }
}