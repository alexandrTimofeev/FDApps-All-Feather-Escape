using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Feeding : MonoBehaviour
{
    public Animator chickenAnimator;
    public Button eatButton;
    public Button drinkButton;
    private bool isFeeding = false;
    public bool isWater = false;
    public string playerTag = "Player"; // Tag to identify the player chicken

    private void Start()
    {
        chickenAnimator = GetComponent<Animator>();
        eatButton.gameObject.SetActive(false);
        drinkButton.gameObject.SetActive(false);

        // Initialize button actions
        eatButton.onClick.AddListener(() => TryToggleFeeding("eat"));
        drinkButton.onClick.AddListener(() => TryToggleFeeding("drink"));
    }

    // Changed from ToggleFeeding to TryToggleFeeding to attempt to feed if this is the player chicken
    private void TryToggleFeeding(string action)
    {
        // Only the player chicken should respond to the feeding action
        if (gameObject.CompareTag(playerTag))
        {
            ToggleFeeding(action);
        }
    }

    private void ToggleFeeding(string action)
    {
        // If already feeding, stop the current action
        if (isFeeding)
        {
            StopFeeding();
        }
        else
        {
            // Start the appropriate coroutine based on the action
            StartCoroutine(action == "eat" ? PlayEatAnimation() : PlayDrinkAnimation());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Only proceed if this is the player chicken
        if (other.CompareTag(playerTag) && !isFeeding)
        {
            // Enable the appropriate button based on the context (isWater)
            eatButton.gameObject.SetActive(!isWater);
            drinkButton.gameObject.SetActive(isWater);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            StopFeeding();
        }
    }

    private void StopFeeding()
    {
        isFeeding = false;
        eatButton.gameObject.SetActive(false);
        drinkButton.gameObject.SetActive(false);
        eatButton.GetComponentInChildren<TMP_Text>().text = "eat";
        drinkButton.GetComponentInChildren<TMP_Text>().text = "drink";
        // Ensure that only the player chicken stops the animation
        if (gameObject.CompareTag(playerTag))
        {
            chickenAnimator.Play("chicken_run");
        }
    }

    private IEnumerator PlayEatAnimation()
    {
        isFeeding = true;
        eatButton.GetComponentInChildren<TMP_Text>().text = "stop";
        chickenAnimator.Play("chicken_eat");
        yield return new WaitForSeconds(4);
        if (gameObject.CompareTag(playerTag))
        {
            StopFeeding();
        }
    }

    private IEnumerator PlayDrinkAnimation()
    {
        isFeeding = true;
        drinkButton.GetComponentInChildren<TMP_Text>().text = "stop";
        chickenAnimator.Play("chicken_drink");
        yield return new WaitForSeconds(5);
        if (gameObject.CompareTag(playerTag))
        {
            StopFeeding();
        }
    }
}
