using UnityEngine;
using UnityEngine.UI;

public class MobileInteractionUI : MonoBehaviour
{
    [SerializeField] private Button pickButton;
    [SerializeField] private Button dropButton;

    private DragWood currentDragWood;

    public void ShowPickDropUI(DragWood dragWood)
    {
        currentDragWood = dragWood;

        pickButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(false);

        pickButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();

        pickButton.onClick.AddListener(() =>
        {
            currentDragWood.Pick();
            pickButton.gameObject.SetActive(false);
            dropButton.gameObject.SetActive(true);
        });

        dropButton.onClick.AddListener(() =>
        {
            currentDragWood.Drop();
            pickButton.gameObject.SetActive(true);
            dropButton.gameObject.SetActive(false);
        });
    }

    public void HideAll()
    {
        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        currentDragWood = null;
    }
}