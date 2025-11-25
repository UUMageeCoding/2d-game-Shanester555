using UnityEngine;
using UnityEngine.UI;

public class MidgameUI : MonoBehaviour
{
    public Text collectableDisplay;

    void Update()
    {
        collectableDisplay.text = "Gears: " + GameManager.collectableCount;
        Debug.Log(GameManager.collectableCount);
    }
}
