using UnityEngine;
using TMPro;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private MoodSystem moodSystem;
    [SerializeField] private TMP_Text dialogueText;

    public void OfferWater()
    {
        moodSystem.ChangeMood(1);

        dialogueText.text =
            "Thanks. Water actually sounds good right now.";
    }

    public void OfferDrink()
    {
        dialogueText.text =
            "I'll have something light, thanks.";
    }

    public void ContinueListening()
    {
        moodSystem.ChangeMood(1);

        dialogueText.text =
            "Thanks for listening. I needed that.";
    }
}