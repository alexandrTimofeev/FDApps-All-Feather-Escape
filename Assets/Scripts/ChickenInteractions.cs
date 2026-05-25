using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChickenInteractions : MonoBehaviour
{
    private Animator chickenAnimator;

    public bool isEating = false;
    public bool isDrinking = false;
    public bool isSleeping = false;
    public bool isLayingEgg = false;
    public bool eggLaidToday = false;
    public bool eggLaidYesterday = true;
    private string currentTag;
    //public GameObject gate;

    private bool aborted = false;
    private float layingEggTime = 50;
    private int progress = 0;
    private float currentProcessTime = 0;

    void Start()
    {
        chickenAnimator = GetComponent<Animator>();
        currentTag = gameObject.tag; // Store the initial tag
    }

    void Update()
    {
        CheckForRoleChange();
        //Debug.Log(gameObject.name + currentTag);// Add this line to check for tag changes
    }
    private void CheckForRoleChange()
    {
        // If the chicken's tag has changed to "Player" from "NPC"
        if (gameObject.tag == "Player" && currentTag == "NPC")
        {
            // If the chicken was sleeping as an NPC, provide the option to stop
            if (isSleeping)
            {
                // Enable the button and set the text to "stop"
                ButtonsController.button.gameObject.SetActive(true);
                ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "stop";

                // Make sure the button stops the sleeping when clicked
                ButtonsController.button.onClick.RemoveAllListeners();
                ButtonsController.button.onClick.AddListener(() => StopSleeping());
            }
        }
        // Update the current tag for the next check
        currentTag = gameObject.tag;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (CompareTag("NPC"))
        {
            return;
        }

        ButtonsController.button.transform.position = other.bounds.center + new Vector3(0, 3);
        ButtonsController.button2.transform.position = other.bounds.center + new Vector3(0, 4);
        ButtonsController.progressBar.transform.position = other.bounds.center + new Vector3(0, 3.1f);

        if (other.CompareTag("Eat"))
        {
            ButtonsController.button.gameObject.SetActive(true);
            ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "eat";
            ButtonsController.button.onClick.RemoveAllListeners();
            ButtonsController.button.onClick.AddListener(delegate () {
                ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "stop";
                StartCoroutine(PlayEatAnimation());
            });

        }
        else if (other.CompareTag("Drink"))
        {
            ButtonsController.button.gameObject.SetActive(true);
            ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "drink";
            ButtonsController.button.onClick.RemoveAllListeners();
            ButtonsController.button.onClick.AddListener(delegate () {
                ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "stop";
                StartCoroutine(PlayDrinkAnimation());
            });
        }
        else if (other.CompareTag("Sleep"))
        {
            if (!isSleeping && !isLayingEgg)
            {
                aborted = false;
                ButtonsController.button.gameObject.SetActive(true);
                if (TimerClock.Hours() >= 5 && TimerClock.Hours() <= 10 && !eggLaidToday)
                    ButtonsController.button2.gameObject.SetActive(true);
                ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "sleep";
                ButtonsController.button2.GetComponentInChildren<TMP_Text>().text = "lay egg";
                ButtonsController.button.onClick.AddListener(() => StartCoroutine(ISleep(other.bounds.center)));
                ButtonsController.button2.onClick.AddListener(() => StartCoroutine(ILayEgg(other)));
            }
            else if (isSleeping)
            {
                //ButtonsController.button.onClick.AddListener(() => StartCoroutine(ISleep(other.bounds.center)));
                return;
            }
            else if (isLayingEgg)
            {
                //ButtonsController.button2.onClick.AddListener(() => StartCoroutine(ILayEgg(other)));
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        ButtonsController.Hide();
        Stop();
    }
    private void Stop()
    {
        //aborted = false;
        ButtonsController.Refresh();
        chickenAnimator.Play("chicken_run");
    }

    IEnumerator PlayEatAnimation()
    {
        isEating = true;
        chickenAnimator.Play("chicken_eat");
        ButtonsController.button.onClick.AddListener(() => Stop());
        yield return new WaitForSeconds(4);
    }

    IEnumerator PlayDrinkAnimation()
    {
        isDrinking = true;
        chickenAnimator.Play("chicken_drink");
        ButtonsController.button.onClick.AddListener(() => Stop());
        yield return new WaitForSeconds(5);
    }
    IEnumerator ISleep(Vector3 bedPos)
    {
        // If already sleeping as an NPC, don't start the coroutine again
        if (isSleeping && gameObject.tag == "NPC")
        {
            yield break;
        }

        isSleeping = true;

        // Set the sleep position only if it's the player to avoid teleporting NPCs
        if (gameObject.tag == "Player")
        {
            transform.position = bedPos + new Vector3(0, -0.12f, 0);
            // Since it's the player, activate the stop button
            ButtonsController.button.gameObject.SetActive(true);
            ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "stop";
            ButtonsController.button.onClick.RemoveAllListeners();
            ButtonsController.button.onClick.AddListener(() => StopSleeping());
        }
        else
        {
            // If it's an NPC, deactivate the buttons
            ButtonsController.button.gameObject.SetActive(false);
            ButtonsController.button2.gameObject.SetActive(false);
        }

        chickenAnimator.Play("chicken_sleep");

        // Continue this loop until the chicken is no longer sleeping or the tag changes
        while (isSleeping && gameObject.tag == "NPC")
        {
            // Ensure the animation keeps playing
            if (!chickenAnimator.GetCurrentAnimatorStateInfo(0).IsName("chicken_sleep"))
            {
                chickenAnimator.Play("chicken_sleep");
            }
            yield return null;
        }

        // If the loop exits, check if it's because the chicken is no longer sleeping
        if (!isSleeping)
        {
            StopSleeping();
        }
        else if (gameObject.tag == "Player")
        {
            // When switching back to player, make sure the stop button is available
            ButtonsController.button.gameObject.SetActive(true);
            ButtonsController.button.GetComponentInChildren<TMP_Text>().text = "stop";
            ButtonsController.button.onClick.RemoveAllListeners();
            ButtonsController.button.onClick.AddListener(() => StopSleeping());
        }
    }

    private void StopSleeping()
    {
        isSleeping = false;
        chickenAnimator.Play("chicken_run");
        ButtonsController.Refresh();
        // Deactivate the stop button
        ButtonsController.button.gameObject.SetActive(false);
    }
    bool Sleep()
    {
        chickenAnimator.Play("chicken_sleep");
        return !isSleeping;
    }
    IEnumerator ILayEgg(Collider2D bed)
    {
        transform.position = bed.bounds.center + new Vector3(0, -0.12f, 0);
        ButtonsController.button.gameObject.SetActive(false);
        ButtonsController.progressBar.gameObject.SetActive(true);
        ButtonsController.button2.GetComponentInChildren<TMP_Text>().text = "stop";
        ButtonsController.button2.onClick.AddListener(() => { 
            isLayingEgg = false;
            aborted = true;
            Debug.Log(aborted);
        });
        if (CompareTag("Player"))
            ButtonsController.progressBar.GetComponent<Image>().sprite = ButtonsController.progressBar.SetProgress(progress);
        isLayingEgg = true;
        yield return new WaitUntil(() => LayEgg());
        progress = 0;
        ButtonsController.Hide();
        Debug.Log(aborted);
        if (!aborted)
        {
            eggLaidToday = true;
            bed.gameObject.transform.Find("egg").gameObject.SetActive(true);
        }
        Stop();
    }
    bool LayEgg()
    {
        if (!isLayingEgg)
            return true;
        chickenAnimator.Play("chicken_lay_egg");
        currentProcessTime += Time.deltaTime;
        if (currentProcessTime >= layingEggTime)
        {
            if (progress >= 10)
            {
                isLayingEgg = false;
                return true;
            }
            currentProcessTime = 0;
            ++progress;
            if (CompareTag("Player"))
                ButtonsController.progressBar.GetComponent<Image>().sprite = ButtonsController.progressBar.SetProgress(progress);
        }
        return false;
    }
}