using UnityEngine;
using UnityEngine.EventSystems;

public class TimeSpeedup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Time.timeScale = 5f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Time.timeScale = 1f;
    }
}
