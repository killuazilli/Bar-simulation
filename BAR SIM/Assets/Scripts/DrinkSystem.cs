using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrinksSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject drinksPanel;
    [SerializeField] private Button[] drinkButtons;

    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;

    private DrinkChoice[] drinkChoices;

    private void Awake()
    {
        CreateDrinkChoices();

        if (drinksPanel != null)
            drinksPanel.SetActive(false);
    }

    private void CreateDrinkChoices()
    {
        drinkChoices = new DrinkChoice[]
        {
            new DrinkChoice
            {
                actionName = "Offer Water",
                choiceID = "water"
            },

            new DrinkChoice
            {
                actionName = "Offer Soft Drink",
                choiceID = "soft_drink"
            },

            new DrinkChoice
            {
                actionName = "Offer Alcohol",
                choiceID = "alcohol"
            },

            new DrinkChoice
            {
                actionName = "Continue Listening",
                choiceID = "listen"
            }
        };
    }

    public void ShowDrinkChoices()
    {
        drinksPanel.SetActive(true);

        for (int i = 0; i < drinkButtons.Length; i++)
        {
            if (i >= drinkChoices.Length)
            {
                drinkButtons[i].gameObject.SetActive(false);
                continue;
            }

            DrinkChoice choice = drinkChoices[i];

            drinkButtons[i].gameObject.SetActive(true);

            TMP_Text buttonText =
                drinkButtons[i].GetComponentInChildren<TMP_Text>();

            buttonText.text = choice.actionName;

            drinkButtons[i].onClick.RemoveAllListeners();

            string id = choice.choiceID;

            drinkButtons[i].onClick.AddListener(
                () => SelectDrink(id)
            );
        }
    }

    private void SelectDrink(string choiceID)
    {
        drinksPanel.SetActive(false);

        dialogueManager.SubmitDrinkChoice(choiceID);
    }

    public void HideDrinkChoices()
    {
        drinksPanel.SetActive(false);
    }
}