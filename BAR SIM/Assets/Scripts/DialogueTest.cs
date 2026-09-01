////using UnityEngine;
//using System.Collections.Generic

//public class DialogueTest : MonoBehaviour
//{
//    [SerializeField] private DialogueManager dialogueManager;

//    public void ShowFirstChoices()
//    {
//        List<DialogueChoice> choices = new List<DialogueChoice>();

//        DialogueChoice supportive = new DialogueChoice
//        {
//            choiceText = "You seem upset. Do you want to talk about it?",
//            npcResponse = "Thanks... I think I actually do need someone to talk to.",
//            moodEffect = 2
//        };

//        DialogueChoice neutral = new DialogueChoice
//        {
//            choiceText = "What would you like to drink?",
//            npcResponse = "I don't know. Anything, really.",
//            moodEffect = 0
//        };

//        DialogueChoice dismissive = new DialogueChoice
//        {
//            choiceText = "Everyone has bad days.",
//            npcResponse = "Yeah... never mind.",
//            moodEffect = -1
//        };

//        choices.Add(supportive);
//        choices.Add(neutral);
//        choices.Add(dismissive);

//        dialogueManager.DisplayChoices(choices);
//    }
