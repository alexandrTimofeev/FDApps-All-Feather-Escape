using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragWood : MonoBehaviour
{
    public float interactDistance = 2f;

    private GameObject player;
    private bool isNear = false;
    private bool isCarried = false;
    private MobileInteractionUI ui;
    private InteractableItem2D interactable;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        interactable = GetComponent<InteractableItem2D>();
        ui = FindObjectOfType<MobileInteractionUI>();
    }

    private void Update()
    {
        if (isCarried || player == null) return;

        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist <= interactDistance && !isNear)
        {
            isNear = true;
            ui?.ShowPickDropUI(this);
        }
        else if (dist > interactDistance && isNear)
        {
            isNear = false;
            ui?.HideAll();
        }
    }

    public void Pick()
    {
        if (isCarried || interactable == null || player == null) return;

        interactable.PickUp(player);
        isCarried = true;
    }

    public void Drop()
    {
        if (!isCarried || interactable == null) return;

        interactable.Drop();
        isCarried = false;
    }
}