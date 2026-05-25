using System.Collections;
using UnityEngine;

public class UIWindowMover : MonoBehaviour, IUIWindowMover
{
    [SerializeField] private RectTransform panel;

    private Coroutine moveCoroutine;

    public void MoveTo(Vector2 targetPosition, float duration)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MovePanelRoutine(targetPosition, duration));
    }

    private IEnumerator MovePanelRoutine(Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }

        panel.anchoredPosition = targetPosition;
        moveCoroutine = null;
    }
}
