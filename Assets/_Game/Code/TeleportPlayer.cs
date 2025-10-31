using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPoint;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = spawnPoint.position;
        }
    }
}
