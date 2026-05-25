using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]
public class InteractableItem2D : MonoBehaviour
{
    [SerializeField] protected float pickRange = 2f;
    protected UIController uiController;
    protected bool isCarrying = false;
    protected GameObject currentPicker;
    protected bool _isOnEscapeZone;

    public bool isOnEscapeZone => _isOnEscapeZone;
    public static event Action<InteractableItem2D> OnItemPickedUp;
    public static event Action<InteractableItem2D> OnItemDropped;

    protected virtual void Start()
    {
        uiController = FindObjectOfType<UIController>();
        if (TryGetComponent(out Collider2D col)) col.isTrigger = true;
    }

    protected virtual void Update()
    {
        if (!isCarrying)
        {
            CheckEscapeZone();

            currentPicker = FindClosestPlayer();
            if (currentPicker == null)
            {
                uiController?.HidePickButton();
                return;
            }

            float distance = Vector2.Distance(currentPicker.transform.position, transform.position);

            if (distance <= pickRange && !uiController.IsCarryingItem)
            {
                uiController.ShowPickButton(transform.position, () => PickUp(currentPicker));
            }
            else
            {
                uiController.HidePickButton();
            }
        }
    }

    protected GameObject FindClosestPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return player != null && Vector2.Distance(player.transform.position, transform.position) <= pickRange * 1.5f
            ? player
            : null;
    }

    public virtual void PickUp(GameObject picker)
    {
        if (isCarrying || uiController.IsCarryingItem) return;

        isCarrying = true;
        currentPicker = picker;
        transform.SetParent(picker.transform);
        transform.localPosition = new Vector3(0, 0.76f, 0);

        OnItemPickedUp?.Invoke(this); // <-- событие
    }


    public virtual void Drop()
    {
        if (!isCarrying) return;

        isCarrying = false;
        transform.SetParent(null);
        currentPicker = null;

        OnItemDropped?.Invoke(this); // <-- событие
    }


    protected virtual void CheckEscapeZone()
    {
        var zone = FindObjectOfType<EscapeZone>();
        _isOnEscapeZone = zone != null && zone.IsOnZone(transform.position);
    }
}