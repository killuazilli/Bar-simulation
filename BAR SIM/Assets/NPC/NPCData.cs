using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "BehindTheBar/NPC Data")]
public class NPCData : ScriptableObject
{
    public string npcName;

    [TextArea(2, 5)]
    public string openingDialogue;

    public int startingMood = 0;

    public Sprite neutralIcon;
    public Sprite positiveIcon;
    public Sprite negativeIcon;
}