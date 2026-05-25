using UnityEngine;

public class BallPhysicsScript : MonoBehaviour
{
    public Rigidbody2D ballRigidbody;
    public float punchForce = 2f;
    public float dynamicCollisionForceMultiplier = 1f;
    public float staticCollisionDamping = 1f;
    public float rotationSpeedMultiplier = 100f; // Control how much the ball rotates relative to its speed

    void FixedUpdate()
    {
        // Update the ball's angular velocity based on its current velocity
        // The faster the ball moves, the faster it should rotate
        ballRigidbody.angularVelocity = ballRigidbody.linearVelocity.magnitude * rotationSpeedMultiplier;
    }

    void PunchBall()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 direction = (mouseWorldPosition - ballRigidbody.position).normalized;
        ballRigidbody.AddForce(direction * punchForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 forceDirection = ballRigidbody.position - collision.contacts[0].point;
        float forceMultiplier = collision.rigidbody == null ? staticCollisionDamping : dynamicCollisionForceMultiplier;
        ballRigidbody.AddForce(forceDirection.normalized * forceMultiplier, ForceMode2D.Impulse);
    }
    private void OnMouseDown()
    {
        PunchBall();
    }
}
