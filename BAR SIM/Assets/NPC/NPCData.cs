using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "BehindTheBar/NPC Data")]
public class NPCData : ScriptableObject
{
    [Header("NPC Information")]
    public string npcName;

    [Header("Dialogue")]
    public TextAsset inkDialogue;

    [Header("Mood")]
    public int startingMood = 0;

    [Header("Mood Icons")]
    public Sprite neutralIcon;
    public Sprite positiveIcon;
    public Sprite negativeIcon;
}