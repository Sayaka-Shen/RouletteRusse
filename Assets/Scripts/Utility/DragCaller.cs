using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragCaller : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public UnityEvent<PointerEventData> OnBeginDragCaller;
    public UnityEvent<PointerEventData> OnDragCaller;
    public UnityEvent<PointerEventData> OnEndDragCaller;
  
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragCaller?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragCaller?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragCaller?.Invoke(eventData);
    }
}
