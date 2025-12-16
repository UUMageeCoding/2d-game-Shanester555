using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenEndingDoor : MonoBehaviour
{
    public bool inTrigger = false;
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;

    // sets the UI for interacting with the door to appear/dissappear if the player enters/exits the trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        inTrigger = true;
        animator1.SetTrigger("Start");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = false;
        animator1.SetTrigger("End");
    }

    void Update()
    {
        // if the player interacts and has at least 7 gears, ending cutscene plays
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.collectableCount >= 7)
            {
                SceneManager.LoadScene("EndingScene1");
                GameManager.timeIsRunning = false;
            }
            // gives players a temporary UI popup to inform them they haven't collected enough gears
            else
            {
                StartCoroutine(RejectAnim());
            }
        }
    }

    private IEnumerator RejectAnim()
    {
        animator2.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        animator2.SetTrigger("End");
    }
}
