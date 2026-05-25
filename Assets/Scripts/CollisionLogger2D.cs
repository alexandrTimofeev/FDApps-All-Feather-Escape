using UnityEngine;

public class CollisionLogger2D : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Столкновение с объектом: " + collision.gameObject.name);
    }
}