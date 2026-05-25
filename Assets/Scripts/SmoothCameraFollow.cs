// SmoothCameraFollow script, attached to the Main Camera
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform targetInit;
    public static Transform target;
    public float smoothSpeed = 0.125f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    private Camera cam;
    private float targetZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
        target = targetInit;
    }

    private void LateUpdate()
    {
        // Camera follow with smooth damp
        Vector3 desiredPosition = target.position + new Vector3(0, 0, -10f); // Assuming you want the camera to stay 10 units above the scene
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10f); // Maintain the camera's z offset

        // Camera zoom with mouse scroll wheel
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }
}
