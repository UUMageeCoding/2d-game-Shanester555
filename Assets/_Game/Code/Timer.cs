using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public Text timeText;

    // Update is called once per frame
    void Update()
    {
        // value increased as time passes, only during gameplay
        if (GameManager.timeIsRunning)
        {
            GameManager.timePassed += Time.deltaTime;
        }
        DisplayTime(GameManager.timePassed);
    }

    void DisplayTime (float timeToDisplay)
    {
        // time is displayed through minutes and seconds
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
