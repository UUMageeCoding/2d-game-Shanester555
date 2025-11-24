using UnityEngine;
using UnityEngine.Rendering;

public class Soundplay : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] float volume;
    [SerializeField] float clipLength;

    private void SoundEffect()
    {
        SoundFXManager.instance.PlaySoundFXClip(clip, transform, volume, clipLength);
    }
}
