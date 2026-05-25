using UnityEngine;
using UnityEngine.UI;

public class Rope : MonoBehaviour, ICarryable
{
    [SerializeField] private float range = 1f;
    [SerializeField] private Button pickButton;
    [SerializeField] private Button dropButton;

    [SerializeField] private Sprite onGround;
    [SerializeField] private Sprite onChick;

    private SpriteRenderer ropeSpriteRenderer;
    private GameObject player;
    private Collider2D col;

    private bool isCarrying = false;
    public bool isOnEscapeZone = false;

    public bool IsCarrying => isCarrying;
    public Transform Transform => transform;

    private static Rope currentActive;

    private void Start()
    {
        ropeSpriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        ropeSpriteRenderer.sprite = onGround;

        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
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
            transform.position = player.transform.position + new Vector3(0, 0.7f);
        }
    }

    void ShowPickButton()
    {
        pickButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(false);

        pickButton.onClick.RemoveAllListeners();
        pickButton.onClick.AddListener(() => CarrySystem.Pick(this));
    }

    void ShowDropButton()
    {
        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(true);

        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(() => CarrySystem.DropCurrent());
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

        ropeSpriteRenderer.sprite = onChick;

        if (col != null)
            col.offset = Vector2.zero;

        ShowDropButton();
    }

    public void Drop()
    {
        isCarrying = false;
        CarrySystem.CurrentItem = null;

        ropeSpriteRenderer.sprite = onGround;
        if (col != null)
            col.enabled = true; 

        //CheckZone();
        HideButtons();
        currentActive = null;
    }

    //void CheckZone()
    //{
    //    EscapeZone zone = FindObjectOfType<EscapeZone>();
    //    if (zone != null)
    //        isOnEscapeZone = zone.IsOnZone(transform.position);
    //}
}
