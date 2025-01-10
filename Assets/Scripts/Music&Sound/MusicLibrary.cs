using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public struct MusicSound
{
    public string trackName;
    public AudioClip musicClip;
}

public class MusicLibrary : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private MusicSound[] _musicSounds;

    public AudioClip GetClipFromName(string name)
    {
        foreach (MusicSound music in _musicSounds)
        {
            if(music.trackName == name)
            {
                return music.musicClip;
            }
        }

        return null;    
    }
}
