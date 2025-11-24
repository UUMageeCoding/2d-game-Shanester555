using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    public Transform player;
    public float maxDistance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float clipLength)
    {
        float distance = Vector3.Distance(player.position, spawnTransform.position);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity); ;

        audioSource.clip = audioClip;

        volume = Mathf.Clamp01(volume - (distance / maxDistance));
        audioSource.volume = volume;

        audioSource.Play();

        if (clipLength == 0)
        {
            clipLength = audioSource.clip.length;
        }

        Destroy(audioSource.gameObject, clipLength);
    }
}
