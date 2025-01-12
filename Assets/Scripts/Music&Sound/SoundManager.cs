using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Settings")]
    [SerializeField] private SoundLibrary _soundLibrary;
    [SerializeField] private AudioSource _soundSource;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound2D(string soundName)
    {
        _soundSource.PlayOneShot(_soundLibrary.GetClipFromName(soundName));
    }
}
