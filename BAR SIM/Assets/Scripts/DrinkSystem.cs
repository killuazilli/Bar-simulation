using UnityEngine;
using TMPro;

public class DrinksSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MoodSystem moodSystem;
    [SerializeField] private TMP_Text dialogueText;

    public void OfferWater()
    {
        moodSystem.ChangeMood(1);

        dialogueText.text =
            "Thanks. Water actually sounds good right now.";
    }

    public void OfferSoftDrink()
    {
        dialogueText.text =
            "I'll have something light, thanks.";
    }

    public void OfferAlcoholicDrink()
    {
        moodSystem.ChangeMood(-1);

        dialogueText.text =
            "I'm not sure that's what I need right now.";
    }

    public void ContinueListening()
    {
        moodSystem.ChangeMood(1);

        dialogueText.text =
            "Thanks for listening. I needed that.";
    }
}