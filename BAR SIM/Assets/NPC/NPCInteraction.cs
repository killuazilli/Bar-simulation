using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private DialogueManager dialogueManager;

    private bool playerNearby;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialogue(npcData);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}