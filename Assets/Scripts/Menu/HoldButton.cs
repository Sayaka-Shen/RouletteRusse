using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float holdDuration;

    [SerializeField, ReadOnly] float holdTime;
    bool isHolding = false;

    [SerializeField] UnityEvent OnHoldStarted;
    [SerializeField] UnityEvent<float> OnHolding;
    [SerializeField] UnityEvent OnHoldCompleted;
    [SerializeField] UnityEvent OnHoldCanceled;

    void Update()
    {
        CheckHold();
    }

    void CheckHold()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
            OnHolding?.Invoke(holdTime / holdDuration);
            if (holdTime > holdDuration)
            {
                OnHoldCompleted?.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        OnHoldStarted?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        if (holdTime < holdDuration)
        {
            OnHoldCanceled?.Invoke();
        }
        holdTime = 0;
    }
}
