using UnityEngine;
using UnityEngine.Rendering;

public class Soundplay : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] float volume;
    [SerializeField] float clipLength;

    // plays referenced sound effect when referenced
    private void SoundEffect()
    {
        SoundFXManager.instance.PlaySoundFXClip(clip, transform, volume, clipLength);
    }
}
