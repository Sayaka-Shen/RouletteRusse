using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text title;
    [SerializeField] ScrollRect scrollRect;

    [Header("Scroll view")]
    [SerializeField] float velocityThresholdToMagnetise = 0.1f;
    bool isMagnetized = true;
    int lastStep;

    [SerializeField, Tooltip("After this time, title will show up again")] float maxAFKTime = 5f;
    float afkTime;

    [Header("Title")]
    [SerializeField, Tooltip("Duration in s for the title to fade in")] float fadeInDuration = 2f;
    [SerializeField, Tooltip("Duration in s for the title to fade out")] float fadeOutDuration = 2f;
    bool isTitleHidden = false;

    const int MIN_PLAYER = 2;
    const int MAX_PLAYER = 6;

    void Start()
    {
        lastStep = GetStep();
        afkTime = maxAFKTime; // Setup to already shown title, but need to be overrided for intro fade
    }

    void Update()
    {
        AFKTimer();
        if (Mathf.Abs(scrollRect.velocity.y) < velocityThresholdToMagnetise && !isMagnetized)
        {
            Debug.Log($"Magnetize on step {GetStep()} with velocity {Mathf.Abs(scrollRect.velocity.y)}");
            isMagnetized = true;
        }
    }

    void AFKTimer()
    {
        afkTime += Time.deltaTime;
        if (afkTime > maxAFKTime)
        {
            ShowTitle();
        }
    }

    void ResetAFKTimer()
    {
        afkTime = 0;
    }

    public void ScrollRectOnValueChanged()
    {
        ResetAFKTimer();

        if (!isTitleHidden)
        {
            HideTitle();
        }
        
        if (GetStep() != lastStep)
        {
            Debug.Log("New step: " + GetStep());
            isMagnetized = false;
            lastStep = GetStep();
        }
    }

    void HideTitle()
    {
        isTitleHidden = true;
        StartCoroutine(Fade(title, Color.clear, fadeOutDuration));
    }

    void ShowTitle()
    {
        isTitleHidden = false;
        StartCoroutine(Fade(title, Color.white, fadeInDuration));
    }

    IEnumerator Fade(MaskableGraphic fadable, Color targetColor, float fadeTime)
    {
        float timer = 0;
        Color baseColor = fadable.color;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            fadable.color = Color.Lerp(baseColor, targetColor, timer / fadeTime);
            yield return null;
        }
    }

    void SelectPlayerCount()
    {
        Debug.Log($"Game start with {GetStep()} players");
    }

    int GetStep()
    {
        return (int)Mathf.Round(Mathf.Clamp01(scrollRect.verticalScrollbar.value) * (MAX_PLAYER - MIN_PLAYER));
    }
}
