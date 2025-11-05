using UnityEngine;

public class SetPlayerSpawn : MonoBehaviour
{
    private PlatformerController platformerController;
    [SerializeField] private string previousSceneName;

    void Start()
    {
        platformerController = GameObject.Find("Player").GetComponent<PlatformerController>();

        if (previousSceneName == platformerController.previousScene)
        {
            platformerController.respawnPoint = transform.position;
        }
    }
}
