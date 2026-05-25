using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheEscape : MonoBehaviour
{
    private List<GameObject> itemsForEscape;

    public Button createItemBut;        // build a catapult
    public Button escapeBut;            // use catapult

    public GameObject catapult;
    private EscapeZone zone;

    private bool allSet = false;

    private void Start()
    {
        zone = FindObjectOfType<EscapeZone>();
        itemsForEscape = new List<GameObject>();
    }
    private void Update()
    {
        if (allSet)
            return;

        if (ReadyForEscape4())
        {
            Debug.Log("Ready for Escape");
            createItemBut.gameObject.transform.position = zone.gameObject.transform.position + new Vector3(0, 3f);
            createItemBut.gameObject.SetActive(true);
            escapeBut.gameObject.SetActive(true);
        }
        else
        {
            itemsForEscape.Clear();
        }
    }
    bool ReadyForEscape4()
    {
        // 2 woods, 1 bucket, 1 rope
        Wood[] woods = FindObjectsOfType<Wood>();
        int woodsCount = 0;
        foreach (var wood in woods)
        {
            if (wood.isOnEscapeZone)
            {
                ++woodsCount;
                if (woodsCount < 3)
                    itemsForEscape.Add(wood.gameObject);
            }
        }
        if (woodsCount < 2)
            return false;
        Rope rope = FindObjectOfType<Rope>();
        Bucket bucket = FindObjectOfType<Bucket>();
        if (rope.isOnEscapeZone && bucket.isOnEscapeZone)
        {
            itemsForEscape.Add(rope.gameObject);
            itemsForEscape.Add(bucket.gameObject);
            return true;
        }
        return false;
    }
    // createItemBut click
    public void CreateCatapult()
    {
        foreach (var item in itemsForEscape)
        {
            item.SetActive(false);
        }
        catapult.transform.position = zone.gameObject.transform.position + new Vector3(-2.33f, -0.95f);
        catapult.SetActive(true);
        zone.gameObject.SetActive(false);
        createItemBut.gameObject.SetActive(false);
        escapeBut.gameObject.transform.position = zone.gameObject.transform.position + new Vector3(0, 3f);
        escapeBut.gameObject.SetActive(true);
        allSet = true;
    }
    // escapeBut click
    public void Escape4()
    {

        StartCoroutine(PlayCatapultAnimation());
        escapeBut.gameObject.SetActive(false);
    }
    IEnumerator PlayCatapultAnimation()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.gameObject.SetActive(false);
        Animator catAnim = catapult.GetComponent<Animator>();
        catAnim.SetTrigger("play");
        while (catAnim.GetCurrentAnimatorStateInfo(0).IsName("catapult"))
        {
            yield return null;
        }
        catAnim.SetTrigger("none");
        yield return new WaitForSeconds(1.17f);
        player.gameObject.transform.position = catapult.gameObject.transform.position + new Vector3(-3.24f, 1.21f);
        player.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        VictoryDetection.won = true;
    }
}
