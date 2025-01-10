using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string soundName;
    public AudioClip soundClip;
}

public class SoundLibrary : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private SoundEffect[] _soundsEffect;

    public AudioClip GetClipFromName(string name)
    {
        foreach (SoundEffect soundEffect in _soundsEffect)
        {
            if(soundEffect.soundName == name)
            {
                return soundEffect.soundClip;
            }
        }

        return null;
    }
}
