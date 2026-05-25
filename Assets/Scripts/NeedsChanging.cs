using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is responsible for needs change
public class NeedsChanging : MonoBehaviour
{
    ChickenInteractions chick;

    public float decreasingSpeedF; // 1 xp decreasing speed (seconds)
    public float recoveringSpeedF; // 1 xp increasing speed (seconds)
    public float decreasingSpeedW;
    public float recoveringSpeedW;
    public float decreasingSpeedS;
    public float recoveringSpeedS;

    private float currentTimeF = 0;
    private float currentTimeW = 0;
    private float currentTimeS = 0;
    private float lowFoodWaterTime = 0; // Таймер для низкого уровня еды/воды

    public int foodXP = 10;
    public int waterXP = 10;
    public int sleepXP = 10;
    public bool eggToday;
    public bool eggYesterday;
    public bool isNugget = false;

    public Sprite nuggetSprite;

    void Start()
    {
        chick = GetComponent<ChickenInteractions>();
        decreasingSpeedF = TimerClock.dayLength * 60 / 10 / 1.7f;
        recoveringSpeedF = 2;
        decreasingSpeedW = TimerClock.dayLength * 60 / 10 / 1.5f;
        recoveringSpeedW = 2;
        decreasingSpeedS = TimerClock.dayLength * 60 / 10 / 1f;
        recoveringSpeedS = TimerClock.dayLength * 60 / 10 / 24 * 8;
    }
    void Update()
    {
        if (!isNugget) // Если ещё не превратились в наггетсы
        {
            UpdateNeeds();
            if (CompareTag("Player"))
                NeedsController.ShowNeeds(this);
        }
    }
    public void UpdateNeeds()
    {
        if (TimerClock.Hours() == 0 && TimerClock.Minutes() == 0)
        {
            chick.eggLaidYesterday = chick.eggLaidToday;
        }
        if (TimerClock.Hours() == 0 && TimerClock.Minutes() == 1)
        {
            chick.eggLaidToday = false;
        }
        eggToday = chick.eggLaidToday;
        eggYesterday = chick.eggLaidYesterday;

        currentTimeF += Time.deltaTime;
        currentTimeW += Time.deltaTime;
        currentTimeS += Time.deltaTime;
        if (chick.isEating)
        {
            if (currentTimeF > recoveringSpeedF)
            {
                currentTimeF = 0;
                if (foodXP < 10)
                    foodXP++;
            }
        }
        if (chick.isDrinking)
        {
            if (currentTimeW > recoveringSpeedW)
            {
                currentTimeW = 0;
                if (waterXP < 10)
                    waterXP++;
            }
        }
        if (chick.isSleeping)
        {
            if (currentTimeS > recoveringSpeedS)
            {
                currentTimeS = 0;
                if (sleepXP < 10)
                    sleepXP++;
            }
        }
        if (!chick.isEating)
        {
            if (currentTimeF > decreasingSpeedF)
            {
                currentTimeF = 0;
                if (foodXP > 1)
                    foodXP--;
            }
        }
        if (!chick.isDrinking)
        {
            if (currentTimeW > decreasingSpeedW)
            {
                currentTimeW = 0;
                if (waterXP > 1)
                    waterXP--;
            }
        }
        if (!chick.isSleeping)
        {
            if (currentTimeS > decreasingSpeedS)
            {
                currentTimeS = 0;
                if (sleepXP > 1)
                    sleepXP--;
            }
        }

        if (foodXP <= 1 || waterXP <= 1)
        {
            lowFoodWaterTime += Time.deltaTime;
            if (lowFoodWaterTime >= 10.0f) // Проверяем, достигли ли мы 40 секунд
            {
                BecomeNugget(); // Превращаемся в наггетсы
            }
        }
        else
        {
            lowFoodWaterTime = 0; // Сброс таймера
        }
    }
    void BecomeNugget()
    {
        isNugget = true;
        gameObject.tag = "NPC"; // Изменяем тег, чтобы предотвратить взаимодействие с объектом

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false; // Отключаем аниматор
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && nuggetSprite != null)
        {
            spriteRenderer.sprite = nuggetSprite; // Меняем спрайт на наггетс
        }

        // По желанию можно добавить задержку перед уничтожением объекта
        Invoke("Disappear", 10.0f); // Уничтожаем объект через 5 секунд, если это необходимо
    }
    void Disappear()
    {
        gameObject.SetActive(false); // Уничтожаем объект
    }
}
