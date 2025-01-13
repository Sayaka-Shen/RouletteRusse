using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class DeathChanceOverlay : MonoBehaviour
{
    [SerializeField] GameObject WrapperDown;
    [SerializeField] GameObject WrapperUp;
    [SerializeField] GameObject WrapperLeft;
    [SerializeField] GameObject WrapperRight;

    [SerializeField] float duration = 1f;

    Sequence sequence;


    private void Awake()
    {
        GameManager.OnDeathChanceChange += OnDeathChanceChange;
        NewTween();
    }

    void OnDeathChanceChange(int newDC)
    {
        sequence.Kill();
        duration = 1 / (6 / newDC);
        NewTween();
    }

    void NewTween()
    {
        sequence = DOTween.Sequence();
        sequence.Append(WrapperDown.transform.DOScaleY(1.2f, duration)).Join(WrapperUp.transform.DOScaleY(1.2f, duration)).Join(WrapperLeft.transform.DOScaleY(1.2f, duration)).Join(WrapperRight.transform.DOScaleY(1.2f, duration))
                .Append(WrapperDown.transform.DOScaleY(1, duration)).Join(WrapperUp.transform.DOScaleY(1, duration)).Join(WrapperLeft.transform.DOScaleY(1, duration)).Join(WrapperRight.transform.DOScaleY(1, duration))
                .SetEase(Ease.InOutSine).SetLoops(-1);
        sequence.Play();
    }
}
