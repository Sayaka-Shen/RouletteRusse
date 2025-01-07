using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text title;
    [SerializeField] ScrollRect scrollRect;

    [Header("General")]
    [SerializeField] GameObject playerCountPrefab;
    [SerializeField, NaughtyAttributes.MinMaxSlider(2, 20)] Vector2Int playerCount = new();
    int deltaPlayerCount;

    [Header("Scroll view")]
    [SerializeField] float velocityThresholdToMagnetise = 0.1f;
    [SerializeField] float magnetizeDuration = 1f;
    Coroutine magnetizeCoroutine;
    int lastStep;

    enum ScrollViewState
    {
        MOVING,
        MAGNETIZING,
        MAGNETIZED
    }

    ScrollViewState scrollViewState = ScrollViewState.MAGNETIZED;

    [SerializeField, Tooltip("After this time, title will show up again")] float maxAFKTime = 5f;
    float afkTime;

    [Header("Title")]
    [SerializeField, Tooltip("Duration in s for the title to fade in")] float fadeInDuration = 2f;
    [SerializeField, Tooltip("Duration in s for the title to fade out")] float fadeOutDuration = 2f;
    bool isTitleHidden = false;

    private void Awake()
    {
        deltaPlayerCount = playerCount.y - playerCount.x;
        lastStep = GetStep();
        afkTime = maxAFKTime; // Setup to already shown title, but need to be overrided for intro fade
        CreatePlayerNumber();
    }

    void Start()
    {

    }

    void Update()
    {
        AFKTimer();

        if (Mathf.Abs(scrollRect.velocity.y) < velocityThresholdToMagnetise && scrollViewState == ScrollViewState.MOVING)
        {
            float targetValue = GetStep() / (float)deltaPlayerCount;
            magnetizeCoroutine = StartCoroutine(MagnetizeToValue(scrollRect, targetValue, magnetizeDuration));
        }
    }

    void CreatePlayerNumber()
    {
        for (int i = playerCount.x; i <= playerCount.y; i++)
        {
            GameObject go = Instantiate(playerCountPrefab, scrollRect.content);
            go.name = i.ToString();
            go.GetComponentInChildren<Text>().text = i.ToString(); // Specific class to have path?
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
        if (scrollViewState == ScrollViewState.MAGNETIZED)
        {
            scrollViewState = ScrollViewState.MOVING;
        }
        
        ResetAFKTimer();

        if (!isTitleHidden)
        {
            HideTitle();
        }

        if (GetStep() != lastStep)
        {
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

    IEnumerator Fade(MaskableGraphic fadable, Color targetColor, float duration)
    {
        float timer = 0;
        Color baseColor = fadable.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadable.color = Color.Lerp(baseColor, targetColor, timer / duration);
            yield return null;
        }
    }

    int GetStep()
    {
        return (int)Mathf.Round(Mathf.Clamp01(scrollRect.verticalScrollbar.value) * deltaPlayerCount);
    }

    IEnumerator MagnetizeToValue(ScrollRect scrollrect, float targetValue, float duration)
    {
        float timer = 0;
        scrollViewState = ScrollViewState.MAGNETIZING;
        float baseValue = scrollrect.verticalNormalizedPosition;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float smoothingFactor = Mathf.SmoothStep(0, 1, timer / duration);
            scrollrect.verticalNormalizedPosition = Mathf.Lerp(baseValue, targetValue, smoothingFactor);
            yield return null;
        }
        scrollViewState = ScrollViewState.MAGNETIZED;
    }

    void SelectPlayerCount()
    {
        Debug.Log($"Game start with {GetStep()} players");
    }
}
