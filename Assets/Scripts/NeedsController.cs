using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class changes needs pictures and current xp
public class NeedsController : MonoBehaviour
{
    public Image foodImageInit;
    public Image waterImageInit;
    public Image sleepImageInit;
    public static Image foodImage;
    public static Image waterImage;
    public static Image sleepImage;

    public static Dictionary<int, Sprite> dict;
    public Sprite xp10;
    public Sprite xp9;
    public Sprite xp8;
    public Sprite xp7;
    public Sprite xp6;
    public Sprite xp5;
    public Sprite xp4;
    public Sprite xp3;
    public Sprite xp2;
    public Sprite xp1;

    public Image egg1ImageInit;
    public Image egg2ImageInit;
    public static Image egg1Image;
    public static Image egg2Image;
    public Sprite eggFullInit;
    public Sprite eggEmptyInit;
    public static Sprite eggFull;
    public static Sprite eggEmpty;

    void Start()
    {
        dict = new()
        {
            { 1, xp1 },
            { 2, xp2 },
            { 3, xp3 },
            { 4, xp4 },
            { 5, xp5 },
            { 6, xp6 },
            { 7, xp7 },
            { 8, xp8 },
            { 9, xp9 },
            { 10, xp10 }
        };
        foodImage = foodImageInit;
        waterImage = waterImageInit;
        sleepImage = sleepImageInit;
        egg1Image = egg1ImageInit;
        egg2Image = egg2ImageInit;
        eggFull = eggFullInit;
        eggEmpty = eggEmptyInit;

    }
    public static void ShowNeeds(NeedsChanging chick)
    {
        foodImage.sprite = dict[chick.foodXP];
        waterImage.sprite = dict[chick.waterXP];
        sleepImage.sprite = dict[chick.sleepXP];
        egg1Image.sprite = chick.eggYesterday ? eggFull : eggEmpty;
        egg2Image.sprite = chick.eggToday ? eggFull : eggEmpty;
    }
}
