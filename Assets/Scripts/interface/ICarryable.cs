using UnityEngine;

public interface ICarryable
{
    void PickUp();
    void Drop();
    bool IsCarrying { get; }
    Transform Transform { get; }
}
