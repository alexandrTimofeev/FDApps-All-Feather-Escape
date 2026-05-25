using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayer : MonoBehaviour
{
    private GameObject currentChicken;
    private AudioManager audioManager;
    public GameObject[] chickens;
    public Button[] chickButtons;
    public Sprite[] defaultSprites;
    public Sprite[] selectedSprites;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        currentChicken = chickens[0];
        currentChicken.tag = "Player";
        currentChicken.GetComponent<Rigidbody2D>().mass = 1;

        for (int i = 1; i < chickens.Length; i++)
        {
            chickens[i].tag = "NPC";
            chickens[i].GetComponent<Rigidbody2D>().mass = 1;
        }
        chickButtons[0].image.sprite = selectedSprites[0];
    }

    void Update()
    {
        for (int i = 0; i < chickens.Length; i++)
        {
            if (chickens[i].GetComponent<NeedsChanging>().isNugget)
            {
                chickButtons[i].interactable = false; // ?????? ?????? ??????????
                chickButtons[i].image.color = new Color(1, 1, 1, 0.5f); // ????????? ????????????????
            }
        }
    }

    public void ChooseChicken(int chickenNumber)
    {
        if (chickens[chickenNumber].GetComponent<NeedsChanging>().isNugget)
            return;

        audioManager.PlaySound(audioManager.buttonClick);
        ButtonsController.Hide();
        ButtonsController.Refresh();
        currentChicken.GetComponent<Animator>().SetFloat("Speed", 0);
        currentChicken.GetComponent<InstantMovementScript>().isMoving = false;

        currentChicken = chickens[chickenNumber];
        currentChicken.tag = "Player";
        currentChicken.GetComponent<Rigidbody2D>().mass = 1;
        chickButtons[chickenNumber].image.sprite = selectedSprites[chickenNumber];

        SmoothCameraFollow.target = currentChicken.GetComponent<Transform>();

        int i = 0;
        foreach (var chicken in chickens)
        {
            if (chicken != currentChicken)
            {
                chicken.tag = "NPC";
                chicken.GetComponent<Rigidbody2D>().mass = 1;
                chickButtons[i].image.sprite = defaultSprites[i];
            }
            ++i;
        }
    }
}
