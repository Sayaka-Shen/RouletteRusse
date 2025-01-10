using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMask : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] HoldButton linkedHoldButton;
    [SerializeField] Tween scaleTween;

    public void StartScaleImage()
    {
        if (scaleTween == null)
        {
            scaleTween.Kill();
        }
        scaleTween = image.transform.DOScaleX(1, linkedHoldButton.HoldDuration);
    }

    public void StopScaleImage()
    {
        scaleTween.Kill();
        scaleTween = image.transform.DOScaleX(0, 0.3f);
    }
}
