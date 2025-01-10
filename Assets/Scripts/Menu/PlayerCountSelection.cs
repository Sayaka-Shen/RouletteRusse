using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class PlayerCountSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text title;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject playerNameSelectionGO;

    [Header("General")]
    [SerializeField] GameObject playerCountPrefab;
    [SerializeField, NaughtyAttributes.MinMaxSlider(2, 20)] Vector2Int playerCount = new();
    int deltaPlayerCount;

    [Header("Scroll view")]
    [SerializeField] float velocityThresholdToMagnetise = 0.1f;
    [SerializeField] float magnetizeDuration = 1f;
    Coroutine magnetizeCoroutine;
    int lastStep;

    [SerializeField, ReadOnly] bool isDragging = false;

    [Header("Shake")]
    [SerializeField] int shakeForceMultiplier;
    Tween tweenShake;
    TweenCallback tweenCallback;
    [SerializeField, ReadOnly] float completionRatio;

    [Header("OnSelectedPlayerCountParam")]
    [SerializeField] float onSelectedPlayerCountDuration;
    [SerializeField] float onSelectedPlayerCountScale;

    enum ScrollViewState
    {
        MOVING,
        MAGNETIZING,
        MAGNETIZED
    }

    [SerializeField, ReadOnly] ScrollViewState scrollViewState = ScrollViewState.MAGNETIZED;


    [Header("Title")]
    [SerializeField, Tooltip("After this time, title will show up again")] float maxAFKTime = 5f;
    float afkTime;
    [SerializeField, Tooltip("Duration in s for the title to fade in")] float fadeInDuration = 2f;
    [SerializeField, Tooltip("Duration in s for the title to fade out")] float fadeOutDuration = 2f;
    bool isTitleHidden = false;

    public static event Action<int> OnPlayerNumberChosen;

    private void Awake()
    {
        deltaPlayerCount = playerCount.y - playerCount.x;
        lastStep = GetStep();
        afkTime = maxAFKTime; // Setup to already shown title, but need to be overrided for intro fade
        CreatePlayerNumber();
        
        tweenCallback = () =>
        {
            tweenShake = scrollRect.transform.DOShakePosition(0.1f, completionRatio * 10, randomnessMode: ShakeRandomnessMode.Harmonic);
            tweenShake.OnComplete(tweenCallback);
        };
    }

    void Update()
    {
        AFKTimer();
        CheckMagnetize();
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
        if (!isDragging)
        {
            afkTime += Time.deltaTime;
            if (afkTime > maxAFKTime)
            {
                ShowTitle();
            }
        }
    }

    void ResetAFKTimer()
    {
        afkTime = 0;
    }

    public void ScrollRectOnValueChanged()
    {
        if (GetStep() != lastStep)
        {
            lastStep = GetStep();
        }

        ResetAFKTimer();

        if (isDragging)
        {
            scrollViewState = ScrollViewState.MOVING;
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

    IEnumerator Fade(MaskableGraphic fadable, Color targetColor, float duration) // COLOR DOTWEEN
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

    int StepToInt(int step)
    {
        return playerCount.y - step;
    }

    void CheckMagnetize()
    {
        if (Mathf.Abs(scrollRect.velocity.y) < velocityThresholdToMagnetise && scrollViewState == ScrollViewState.MOVING && !isDragging)
        {
            float targetValue = GetStep() / (float)deltaPlayerCount;
            magnetizeCoroutine = StartCoroutine(MagnetizeToValue(scrollRect, targetValue, magnetizeDuration));
        }
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

    public void StartShake()
    {
        completionRatio = 0;
        tweenCallback.Invoke();
    }

    public void ContinuousShake(float completionRatio)
    {
        this.completionRatio = completionRatio;
    }

    public void StopShake()
    {
        completionRatio = 0;
        tweenShake.Rewind();
    }
    
    public void SelectPlayerCount()
    {
        StopShake(); // Need another tween, maybe explosion like, or fade
        Sequence selectedSequence = DOTween.Sequence();
        selectedSequence.Append(scrollRect.transform.DOScale(onSelectedPlayerCountScale, onSelectedPlayerCountDuration));
        selectedSequence.Append(canvasGroup.DOFade(0, onSelectedPlayerCountDuration));
        selectedSequence.onComplete += () =>
        {
            playerNameSelectionGO.SetActive(true);
            OnPlayerNumberChosen?.Invoke(StepToInt(GetStep()));
            gameObject.SetActive(false);
        };
        selectedSequence.Play();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        if (!isTitleHidden)
        {
            HideTitle();
            isTitleHidden = true;
        }
        if (magnetizeCoroutine != null)
        {
            StopCoroutine(magnetizeCoroutine);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
