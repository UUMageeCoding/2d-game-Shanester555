using UnityEngine;
using UnityEngine.UI;

public class MidgameUI : MonoBehaviour
{
    public Text collectableDisplay;

    void Update()
    {
        // displays number of gears the player has collected
        collectableDisplay.text = "Gears: " + GameManager.collectableCount;
    }
}
