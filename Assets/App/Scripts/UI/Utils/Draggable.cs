using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    Vector3 offset;

    public void OnBeginDrag (BaseEventData eventData)
    {
        offset = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
    }

    public void OnDrag (BaseEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) - offset;
    }

    public void OnPointerDown (BaseEventData eventData)
    {
        transform.SetAsLastSibling ();
    }
}
