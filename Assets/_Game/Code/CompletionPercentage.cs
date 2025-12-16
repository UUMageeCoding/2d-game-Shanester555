using UnityEngine;
using UnityEngine.UI;

public class CompletionPercentage : MonoBehaviour
{
    // attempted to have percentage shown, scrapped in favor of fraction
    public Text percentageText;
    public float percentage;

    private void Update()
    {
        DisplayText();
    }

    void DisplayText()
    {

        percentageText.text = GameManager.collectableCount + "/13";
    }
}
