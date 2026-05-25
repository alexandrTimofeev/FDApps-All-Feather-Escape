using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    public Collider2D ballCollider; // Assign the ball's collider here in the inspector
    public float verticalOffset; // How much lower the shadow should be from the ball

    void LateUpdate()
    {
        if (ballCollider != null)
        {
            // Use the center of the collider to position the shadow
            Vector2 colliderCenter = ballCollider.bounds.center;
            transform.position = new Vector3(colliderCenter.x, colliderCenter.y - verticalOffset, transform.position.z);
        }

        // Keep the shadow's rotation constant
        transform.rotation = Quaternion.identity;
    }
}
