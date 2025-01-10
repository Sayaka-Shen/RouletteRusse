using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Settings")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private MusicLibrary _musicLibrary;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.1f)
    {
        StartCoroutine(AnimateMusicCrossFade(_musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDuration = 0.1f)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            _musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }
        
        _musicSource.clip = nextTrack;
        _musicSource.Play();
        
        percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            _musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
}
