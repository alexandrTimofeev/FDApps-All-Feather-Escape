using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button pickButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private RectTransform pickButtonTransform;

    public bool IsCarryingItem { get; private set; }

    private void Start()
    {
        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InteractableItem2D.OnItemPickedUp += HandlePickUp;
        InteractableItem2D.OnItemDropped += HandleDrop;
    }

    private void OnDisable()
    {
        InteractableItem2D.OnItemPickedUp -= HandlePickUp;
        InteractableItem2D.OnItemDropped -= HandleDrop;
    }

    private void HandlePickUp(InteractableItem2D item)
    {
        SetCarryingState(true);
        ShowDropButton(item.Drop);
        HidePickButton();
    }

    private void HandleDrop(InteractableItem2D item)
    {
        SetCarryingState(false);
        HideDropButton();
    }


    public void ShowPickButton(Vector3 worldPos, UnityAction action)
    {
        if (IsCarryingItem) return;

        pickButtonTransform.position = Camera.main.WorldToScreenPoint(worldPos);
        pickButton.onClick.RemoveAllListeners();
        pickButton.onClick.AddListener(action);
        pickButton.gameObject.SetActive(true);
    }

    public void HidePickButton() => pickButton.gameObject.SetActive(false);

    public void ShowDropButton(UnityAction action)
    {
        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(action);
        dropButton.gameObject.SetActive(true);
    }

    public void HideDropButton() => dropButton.gameObject.SetActive(false);

    public void SetCarryingState(bool state) => IsCarryingItem = state;
}