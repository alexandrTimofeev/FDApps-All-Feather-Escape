using UnityEngine;
using UnityEngine.UI;

public class EscapeZone : MonoBehaviour
{
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    public float range = 6f;

    private Vector3 targetPos;
    private bool waitingForPlacement = false;

    public Button setZoneButton;

    void Start()
    {
        xmin += range;
        xmax -= range;
        ymin += range;
        ymax -= range;

        setZoneButton.gameObject.SetActive(true);
        setZoneButton.onClick.AddListener(EnablePlacement);
    }

    void Update()
    {
        if (waitingForPlacement && Input.GetMouseButtonDown(0)) // ЛКМ
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1));
            targetPos = worldPosition;

            if (targetPos.x < xmin || targetPos.x > xmax || targetPos.y > ymax || targetPos.y < ymin)
            {
                transform.position = targetPos;
                waitingForPlacement = false;
                Debug.Log("Зона установлена!");
            }
            else
            {
                Debug.Log("Выбранная точка вне границ допустимой зоны.");
            }
        }
    }

    

    void EnablePlacement()
    {
        waitingForPlacement = true;
        Debug.Log("Ожидаем клик для установки EscapeZone...");
    }

    public bool IsOnZone(Vector3 pos)
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Bounds bounds = col.bounds;

        bool isInside = bounds.Contains(pos);
        Debug.Log(isInside ? "yes" : "no");
        return isInside;
    }
}