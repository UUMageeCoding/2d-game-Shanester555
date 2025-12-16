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
        // gets distance from player to source of sound
        float distance = Vector3.Distance(player.position, spawnTransform.position);

        // doesn't instantiate sound effect if player is too far away to hear it
        if (distance >= maxDistance)
        {
            return;
        }

        // instantiates sound object at location of object making the noise
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity); ;

        audioSource.clip = audioClip;

        // volume becomes weaker in reference to how far the player is from the source
        volume = Mathf.Clamp01(volume - (distance / maxDistance));
        audioSource.volume = volume;

        audioSource.Play();

        // if clip length isn't specified, plays the full clip instead
        if (clipLength == 0)
        {
            clipLength = audioSource.clip.length;
        }

        Destroy(audioSource.gameObject, clipLength);
    }
}
