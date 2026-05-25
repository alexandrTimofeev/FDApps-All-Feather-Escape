using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    public static Button button;
    public static Button button2;
    public static ProgressController progressBar;

    public Button buttonInit;
    public Button button2Init;
    public GameObject progressBarInit;
    void Start()
    {
        button = buttonInit;
        button2 = button2Init;
        progressBar = progressBarInit.GetComponent<ProgressController>();
        Hide();
    }
    public static void Hide()
    {
        button.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
    }
    public static void Refresh()
    {
        button.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
    }
}
