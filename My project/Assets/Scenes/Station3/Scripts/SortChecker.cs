using UnityEngine;
using UnityEngine.UI;  // For UI Text or other UI elements if you want feedback display

public class SortChecker : MonoBehaviour
{
    public DraggableFood[] allFoods;  // Assign all food prefabs in Inspector

    public GameObject successFeedback; // Assign UI element or effect for success
    public GameObject failFeedback;    // Assign UI element or effect for fail

    public float resetDelay = 2f;      // Seconds before resetting on fail

    // Called by your Check Button UI
    public void CheckSorting()
    {
        bool allCorrect = true;

        foreach (DraggableFood food in allFoods)
        {
            bool isCorrect = (food.hasGrain && food.currentBasketTag == "CorrectBasket")
                          || (!food.hasGrain && food.currentBasketTag == "WrongBasket");

            if (!isCorrect)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            ShowSuccessFeedback();
            // You can load next level here or trigger next event
        }
        else
        {
            ShowFailFeedback();
            Invoke(nameof(ResetLevel), resetDelay);
        }
    }

    void ShowSuccessFeedback()
    {
        if (successFeedback != null)
            successFeedback.SetActive(true);

        if (failFeedback != null)
            failFeedback.SetActive(false);
    }

    void ShowFailFeedback()
    {
        if (failFeedback != null)
            failFeedback.SetActive(true);

        if (successFeedback != null)
            successFeedback.SetActive(false);
    }

    void ResetLevel()
    {
        // Reset all foods to start positions and clear basket assignment
        foreach (DraggableFood food in allFoods)
        {
            food.ResetPosition();
        }

        // Hide feedback after reset
        if (failFeedback != null)
            failFeedback.SetActive(false);
    }
}
