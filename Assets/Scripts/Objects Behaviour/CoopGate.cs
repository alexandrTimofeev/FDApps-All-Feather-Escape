using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopGate : MonoBehaviour
{
    public GameObject openedGate;
    public Transform[] LeavingCoopPoints;
    public Transform[] EnteringCoopPoints;

    void Start()
    {
        gameObject.SetActive(true);
        openedGate.SetActive(false);
    }
    void Update()
    {
        if (TimerClock.Hours() == 11 && TimerClock.Minutes() == 56)
        {
        }
        else if (TimerClock.Hours() == 12)
        {
            gameObject.SetActive(false);
        }
        else if (TimerClock.Hours() == 22)
        {
            gameObject.SetActive(true);
        }
    }
}
