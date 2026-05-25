using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlanMenu : MonoBehaviour
{
    public GameObject planMenuCanvas;
    public GameObject hidePanel;
    public GameObject pauseButton;
    public static int planNumber = 1;
    public void TogglePlanMenu()
    {
        bool isActive = planMenuCanvas.activeSelf;
        planMenuCanvas.SetActive(!isActive);
        pauseButton.SetActive(isActive);
        hidePanel.SetActive(isActive);

    }
    public void ChoosePlan(int number)
    {
        planNumber = number;
    }
}
