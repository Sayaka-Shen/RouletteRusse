using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] public float HoldDuration { get; private set; }

    [SerializeField, ReadOnly] float holdTime;
    [SerializeField, ReadOnly] bool isHolding = false;

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
            OnHolding?.Invoke(holdTime / HoldDuration);
            if (holdTime >= HoldDuration)
            {
                ResetHold();
                OnHoldCompleted?.Invoke();
            }
        }
    }

    void ResetHold() // Reset state to next input
    {
        isHolding = false; 
        holdTime = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        OnHoldStarted?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetHold();
        if (holdTime < HoldDuration)
        {
            OnHoldCanceled?.Invoke();
        }
    }
}
