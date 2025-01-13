using System;
using System.Collections.Generic;
using UnityEngine;

public class VolumeButton : MonoBehaviour
{
    public static VolumeButton Instance;
    
    public bool m_bGetVolumeFromPhone = true;

    private float m_fPrevVolume = -1;
    private bool m_bShutDown = false;

    public static event Action OnVolumeUp;
    public static event Action OnVolumeDown;

    //Get phone volume if running or android or application volume if running on pc
    //(or wanted by user)
    public float GetVolume()
    {
        if (m_bGetVolumeFromPhone && Common.IsRunningOnAndroid())
        {
            AndroidJavaObject audioManager = Common.GetAndroidAudioManager();
            return audioManager.Call<int>("getStreamVolume", 3);
        }
        else
        {
            return AudioListener.volume;
        }

    }

    //set phone or application volume (according if running on android or if user want application volume)
    public void SetVolume(float a_fVolume)
    {
        if (m_bGetVolumeFromPhone && Common.IsRunningOnAndroid())
        {
            AndroidJavaObject audioManager = Common.GetAndroidAudioManager();
            audioManager.Call("setStreamVolume", 3, (int)a_fVolume, 0);
        }
        else
        {
            AudioListener.volume = a_fVolume;
        }
    }

    private void ResetVolume()
    {
        SetVolume(m_fPrevVolume);
    }

    void Start()
    {
        PowerOn();
    }

    //If user want to change volume, he has to mute this script first
    //else the script will interpret this has a user input and resetvolume
    public void ShutDown()
    {
        m_bShutDown = true;
    }

    //to unmute the script
    public void PowerOn()
    {
        m_bShutDown = false;
        //get the volume to avoid interpretating previous change (when script was muted) as user input
        m_fPrevVolume = GetVolume();

        //if volume is set to max, reduce it -> if not, there will be no detection for volume up
        if (m_fPrevVolume == GetMaxVolume())
        {
            --m_fPrevVolume;
            SetVolume(m_fPrevVolume);
        }
    }

    //Get max volume phone
    public float GetMaxVolume()
    {
        if (m_bGetVolumeFromPhone && Common.IsRunningOnAndroid())
        {
            AndroidJavaObject audioManager = Common.GetAndroidAudioManager();
            return audioManager.Call<int>("getStreamMaxVolume", 3);
        }
        else
        {
            return 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bShutDown)
            return;

        float fCurrentVolume = GetVolume();
        float fDiff = fCurrentVolume - m_fPrevVolume;

        //if volume change, compute the difference and call listener according to
        if (fDiff < 0)
        {
            ResetVolume();
            OnVolumeDown?.Invoke();
            Debug.Log("down");
        }
        else if (fDiff > 0)
        {
            ResetVolume();
            OnVolumeUp?.Invoke();
            Debug.Log("up");
        }
    }
}
