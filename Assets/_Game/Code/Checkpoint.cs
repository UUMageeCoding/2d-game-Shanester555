using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private RespawnScript respawn;

    void Awake()
    {
        respawn = GetComponent<RespawnScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
            Debug.Log("It worked!");
        }
    }
}
