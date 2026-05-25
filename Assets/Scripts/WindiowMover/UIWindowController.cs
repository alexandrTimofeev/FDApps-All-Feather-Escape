using UnityEngine;

public class UIWindowController : MonoBehaviour
{
    [SerializeField] private Vector2 shownPosition;
    [SerializeField] private Vector2 hiddenPosition;
    [SerializeField] private float moveDuration = 0.5f;

    private bool isShown = true;
    private IUIWindowMover mover;

    private void Awake()
    {
        mover = GetComponent<IUIWindowMover>();
        if (mover == null)
            Debug.LogError("UIWindowMover не найден!");
    }

    public void Toggle()
    {
        Vector2 target = isShown ? hiddenPosition : shownPosition;
        mover.MoveTo(target, moveDuration);
        isShown = !isShown;
    }
}
