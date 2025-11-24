using UnityEngine;
using UnityEngine.Rendering;

public class Soundplay1 : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] float volume;
    [SerializeField] float clipLength;

    private void SoundEffect1()
    {
        SoundFXManager.instance.PlaySoundFXClip(clip, transform, volume, clipLength);
    }
}
