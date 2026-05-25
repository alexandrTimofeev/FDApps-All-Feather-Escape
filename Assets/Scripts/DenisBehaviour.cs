using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenisBehaviour : MonoBehaviour
{
    private Animator animator;
    private float animationSpeed = 0.26f;
    private int hours;
    private int minutes;
    public float speed = 3f;
    private float range = 0.1f;
    private bool inProgress = false;
    private bool isStanding = false;
    private bool isWatering = false;
    private float wateringTimer = 0;
    private float wateringTime = 7f;
    private float standingTimer = 0;
    private float standingTime = 7f;
    private float pickingEggTime = 3f;

    public List<Transform> routePickEggs;
    public List<Transform> routeToBarn;
    public List<Transform> routeFromBarn;
    public List<Transform> routeWaterGarden;
    public List<Transform> routeBringChicksHome;
    public List<Transform> routeFromCoop;
    private int currentDestination = 0;
    private Vector2 way;

    public GameObject wateringCan;
    public Pit pit;

    void Start()
    {
        animator = GetComponent<Animator>();
        HideAllPoints();
    }
    void Update()
    {
        if (inProgress)
            return;

        hours = TimerClock.Hours();
        minutes = TimerClock.Minutes();

        currentDestination = 0;
        if (hours == 11 && minutes == 0)
        {
            way = routePickEggs[0].position;
            MirrorAnimation();
            StartCoroutine(IPickEggs());
        }
        /*
        if (hours == 13 && minutes == 0)
        {
            way = routeToBarn[0].position;
            MirrorAnimation();
            StartCoroutine(IGoToBarn());
        }

        if (hours == 15 && minutes == 0)
        {
            way = routeFromBarn[0].position;
            MirrorAnimation();
            StartCoroutine(IGoFromBarn());
        }
        */
        if (hours == 16 && minutes == 0)
        {
            way = routeWaterGarden[0].position;
            MirrorAnimation();
            StartCoroutine(IWaterGarden());
        }
        if (hours == 20 && minutes == 0)
        {
            //way = routeBringChicksHome[0].position;
            //MirrorAnimation();
            //StartCoroutine(GoSomeWhere(BringChicksHome()));
        }
    }
    private void HideAllPoints()
    {
        HideAllPointsExceptSome(ref routePickEggs, new List<int> { 8, 9, 10, 11, 14, 15, 16, 17 });
        HideAllPoints(ref routeToBarn);
        HideAllPoints(ref routeFromBarn);
        HideAllPoints(ref routeWaterGarden);
        HideAllPoints(ref routeBringChicksHome);
    }
    private void HideAllPoints(ref List<Transform> list)
    {
        foreach (var item in list) 
        {
            item.gameObject.SetActive(false);
        }
    }
    private void HideAllPointsExceptSome(ref List<Transform> list, List<int> notHidden)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (!notHidden.Contains(i))
                list[i].gameObject.SetActive(false);
        }
    }
    private void MirrorAnimation()
    {
        if (way.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (way.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);

    }
    private string ChooseAnimation()
    {
        animator.speed = 1;
        if (isStanding)
            return "back_stand";
        else if (Mathf.Abs(way.x - transform.position.x) < Mathf.Abs(way.y - transform.position.y) &&
            transform.position.y < way.y)
            return "back_walk";
        else if (Mathf.Abs(way.x - transform.position.x) < Mathf.Abs(way.y - transform.position.y) &&
            transform.position.y >= way.y)
            return "towards_walk";
        else
            return "side_walk";
    }
    private string ChooseAnimationWithCan()
    {
        animator.speed = animationSpeed;
        if (isStanding)
            return "back_stand_can 2";
        else if (isWatering)
            return "watering 1";
        else if (Mathf.Abs(way.x - transform.position.x) < Mathf.Abs(way.y - transform.position.y) &&
            transform.position.y < way.y)
            return "back_walk_can 1";
        else if (Mathf.Abs(way.x - transform.position.x) < Mathf.Abs(way.y - transform.position.y) &&
            transform.position.y >= way.y)
            return "towards_walk_can 1";
        else
            return "side_walk_can 1";
    }
    IEnumerator IGoToBarn()
    {
        inProgress = true;
        yield return new WaitUntil(() => GoToBarn());
        inProgress = false;
    }
    bool GoToBarn()
    {
        animator.Play(ChooseAnimation());
        transform.position = Vector2.MoveTowards(transform.position, way, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, way) < range)
        {
            ++currentDestination;
            if (currentDestination >= routeToBarn.Count)
            {
                return true;
            }
            way = routeToBarn[currentDestination].position;
            MirrorAnimation();
        }
        return false;
    }
    IEnumerator IGoFromBarn()
    {
        inProgress = true;
        yield return new WaitUntil(() => GoFromBarn());
        inProgress = false;
    }
    bool GoFromBarn()
    {
        animator.Play(ChooseAnimation());
        transform.position = Vector2.MoveTowards(transform.position, way, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, way) < range)
        {
            ++currentDestination;
            if (currentDestination >= routeFromBarn.Count)
            {
                return true;
            }
            way = routeFromBarn[currentDestination].position;
            MirrorAnimation();
        }
        return false;
    }
    IEnumerator IWaterGarden()
    {
        inProgress = true;
        yield return new WaitUntil(() => WaterGarden());
        inProgress = false;
    }
    bool WaterGarden()
    {
        if (currentDestination < 6 || currentDestination > 28)
            animator.Play(ChooseAnimation());
        else
            animator.Play(ChooseAnimationWithCan());

        if (isWatering)
        {
            animator.speed = 0.16f;
            wateringTimer += Time.deltaTime;
            if (wateringTimer >= wateringTime)
            {
                wateringTimer = 0;
                isWatering = false;
            }
        }
        else if (isStanding)
        {
            standingTimer += Time.deltaTime;
            if (standingTimer >= standingTime)
            {
                standingTimer = 0;
                isStanding = false;
                pit.Animate();
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, way, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, way) < range)
            {
                // Denis reached the watering can
                if (currentDestination == 5)
                    wateringCan.SetActive(false);

                // Time to get water from the pit
                else if (currentDestination == 6)
                {
                    // Набрать воду в колодце
                    isStanding = true;
                    pit.Animate();
                }

                // Watering starts
                else if (currentDestination == 9 || currentDestination == 12 || currentDestination == 15 || 
                    currentDestination == 18 || currentDestination == 21 || currentDestination == 24)
                    isWatering = true;

                // Time to place the watering can
                else if (currentDestination == 28)
                    wateringCan.SetActive(true);

                ++currentDestination;
                if (currentDestination >= routeWaterGarden.Count)
                {
                    return true;
                }
                way = routeWaterGarden[currentDestination].position;
                MirrorAnimation();
            }
        }
        return false;
    }
    IEnumerator IPickEggs()
    {
        inProgress = true;
        yield return new WaitUntil(() => PickEggs());
        inProgress = false;
    }
    bool PickEggs()
    {
        animator.Play(ChooseAnimation());

        if (isStanding)
        {
            standingTimer += Time.deltaTime;
            if (standingTimer >= pickingEggTime)
            {
                standingTimer = 0;
                isStanding = false;
            }
            return false;
        }

        transform.position = Vector2.MoveTowards(transform.position, way, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, way) < range)
        {
            if (currentDestination >= 8 && currentDestination <= 11 || currentDestination >= 14 && currentDestination <= 17)
            {
                if (routePickEggs[currentDestination].Find("egg").gameObject.activeInHierarchy)
                {
                    isStanding = true;
                    routePickEggs[currentDestination].Find("egg").gameObject.SetActive(false);
                }
            }
            ++currentDestination;
            if (currentDestination >= routePickEggs.Count)
                return true;
            way = routePickEggs[currentDestination].position;
            if (currentDestination != 12 && currentDestination != 18)
                way += new Vector2(0, -0.85f);
            MirrorAnimation();
        }
        return false;
    }
}
