using UnityEngine;
using TMPro;

public class TimerClock : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text dayText;
    private int day = 1;
    public static float dayLength = 11f; // Duration of one in-game day in real-world minutes
    public static float currentTime;
    private float timeScale; // Time scale to control the speed of in-game time

    void Start()
    {
        currentTime = 9f * 60; // Start at 9:00 AM
        timeScale = 1440 / (dayLength * 60); // Calculate time scale based on dayLength
        UpdateTimeText();
        UpdateDayText();
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeScale;
        if (Mathf.FloorToInt(currentTime % 60) % 15 == 0)
        {
            UpdateTimeText();
        }

        if (Mathf.FloorToInt(currentTime / 60) >= 24)
        {
            currentTime -= 1440; // Reset the time after a full day
            ++day;
            UpdateDayText();
        }
    }

    void UpdateTimeText()
    {
        int hours = Mathf.FloorToInt(currentTime / 60);
        int minutes = Mathf.FloorToInt(currentTime % 60);
        string timeString = string.Format("{0:D2}:{1:D2}", hours, minutes);
        timeText.text = timeString;
    }

    void UpdateDayText()
    {
        string dayString = "day " + day;
        dayText.text = dayString;
    }

    public static int Hours()
    {
        return Mathf.FloorToInt(currentTime / 60);
    }

    public static int Minutes()
    {
        return Mathf.FloorToInt(currentTime % 60);
    }
}
