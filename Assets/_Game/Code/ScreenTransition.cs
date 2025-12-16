using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public GameObject player;
    public Transform target;

    public CinemachineConfiner2D confiner;
    public Collider2D newBounds;

    [SerializeField] Animator animator;
    [SerializeField] float timer = 1f;
    [SerializeField] Rigidbody2D rb;

    public PlatformerController platformerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportAndChangeBounds());
        }
    }

    private IEnumerator TeleportAndChangeBounds()
    {
        // screen fades to black
        animator.SetTrigger("End");
        // stops player fro moving
        GameManager.canMove = false;

        // waits for screen to fully fade to black before moving camera
        yield return new WaitForSeconds(timer);

        // Optionally disable confiner momentarily
        confiner.BoundingShape2D = null;

        // Assign new confiner bounds
        confiner.BoundingShape2D = newBounds;

        // sends player to the next screen
        player.transform.position = target.position;

        rb.transform.eulerAngles = new Vector3(rb.transform.eulerAngles.x, rb.transform.eulerAngles.y, 0);

        // waits for camera to move before fading screen in 
        yield return new WaitForSeconds(timer);

        // screen fades back in
        animator.SetTrigger("Start");

        GameManager.canMove = true;
    }
}