using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenEndingDoor : MonoBehaviour
{
    public bool inTrigger = false;
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;

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
