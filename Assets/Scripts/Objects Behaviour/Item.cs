using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, ICarryable
{
    [SerializeField] private float range = 1f;
    [SerializeField] private Button pickButton;
    [SerializeField] private Button dropButton;

    private GameObject player;
    private bool isCarrying = false;
    public bool isOnEscapeZone = false;

    public bool IsCarrying => isCarrying;
    public Transform Transform => transform;

    private static Item currentActive;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (!isCarrying && dist < range && (currentActive == null || currentActive == this))
        {
            ShowPickButton();
            currentActive = this;
        }
        else if (!isCarrying && currentActive == this && dist >= range)
        {
            HideButtons();
            currentActive = null;
        }

        if (isCarrying)
        {
            transform.position = player.transform.position + new Vector3(0, 0.76f);
        }
    }

    void ShowPickButton()
    {
        pickButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(false);

        pickButton.onClick.RemoveAllListeners();
        pickButton.onClick.AddListener(() =>
        {
            CarrySystem.DropCurrent();
            CarrySystem.Pick(this);
            PickUp();
        });
    }

    void ShowDropButton()
    {
        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(true);

        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(() =>
        {
            CarrySystem.DropCurrent();
            Drop();
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EscapeZone>(out EscapeZone zone))
        {
            isOnEscapeZone = true;
            Debug.Log("Предмет оказался в зоне побега!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EscapeZone>(out EscapeZone zone))
        {
            isOnEscapeZone = false;
            Debug.Log("Предмет вышел из зоны побега");
        }
    }

    void HideButtons()
    {
        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    public void PickUp()
    {
        isCarrying = true;
        CarrySystem.CurrentItem = this;
        ShowDropButton();
    }

    public void Drop()
    {
        isCarrying = false;
        CarrySystem.CurrentItem = null;
        currentActive = null;
        //CheckZone();
        HideButtons();
    }

    //void CheckZone()
    //{
    //    EscapeZone zone = FindObjectOfType<EscapeZone>();
    //    if (zone != null)
    //        isOnEscapeZone = zone.IsOnZone(transform.position);
    //}
}
