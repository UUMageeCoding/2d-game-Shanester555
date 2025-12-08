using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenEndingDoor : MonoBehaviour
{
    public bool inTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = false;
    }

    void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.collectableCount >= 7 && GameManager.collectableCount != 13)
            {
                SceneManager.LoadScene("EndingScene1");
                GameManager.timeIsRunning = false;
            }
            else if (GameManager.collectableCount == 13)
            {
                SceneManager.LoadScene("EndingScene2");
                GameManager.timeIsRunning = false;
            }
        }
    }
}
