using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform target;

    public CinemachineConfiner2D confiner;
    public Collider2D newBounds;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportAndChangeBounds());
        }
    }

    private IEnumerator TeleportAndChangeBounds()
    {
        // Teleport the player
        player.transform.position = target.position;

        // Optionally disable confiner momentarily
        confiner.BoundingShape2D = null;

        // Wait 1 second (optional, can adjust)
        yield return new WaitForSeconds(3f);

        // Assign new confiner bounds
        confiner.BoundingShape2D = newBounds;
    }
}