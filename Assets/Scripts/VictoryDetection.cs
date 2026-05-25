using UnityEngine;

public class VictoryDetection : MonoBehaviour
{
    public GameObject victoryScreenCanvas; // Assign this in the Unity Editor

    // Define the boundaries based on the coordinates provided
    private float minX = -18f;
    private float maxX = 34f;
    private float minY = -19f;
    private float maxY = 9f;

    public static bool won = false;
    // Update is called once per frame
    void Update()
    {
        if (won)
        {
            victoryScreenCanvas.SetActive(true);

            PlayerPrefs.SetInt("GameWon", 1);
            PlayerPrefs.Save(); 

            won = false;
        }
        /*
        if (!IsWithinBounds(transform.position))
        {
            // Player has escaped, show the victory screen
            victoryScreenCanvas.gameObject.SetActive(true);
        }
        */
    }

    private bool IsWithinBounds(Vector3 position)
    {
        // Check if the player's position is within the given boundaries
        return position.x >= minX && position.x <= maxX &&
               position.y >= minY && position.y <= maxY;
    }
}
