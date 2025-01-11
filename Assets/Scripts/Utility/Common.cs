using UnityEngine;

public static class Common
{

    private static AndroidJavaObject s_mainActivity = null;
    private static AndroidJavaObject s_androidAudioManager = null;

    public static AndroidJavaObject GetAndroidAudioManager()
    {
        if (s_androidAudioManager == null)
        {
            s_androidAudioManager = GetMainActivity().Call<AndroidJavaObject>("getSystemService", "audio");
        }
        return s_androidAudioManager;
    }

    public static AndroidJavaObject GetMainActivity()
    {
        if (s_mainActivity == null)
        {
            var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            s_mainActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return s_mainActivity;
    }

    public static bool IsRunningOnAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
