using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Провайдер ввода с акселерометра с поддержкой калибровки нулевого положения.
/// Раздельные мёртвые зоны для горизонтальной и вертикальной осей.
/// </summary>
public class AccelerometerInputProvider : MonoBehaviour, IInputProvider
{
    [Header("Привязка осей устройства к управлению")]
    [SerializeField] private AxisBinding horizontalAxis = AxisBinding.X;
    [SerializeField] private AxisBinding verticalAxis = AxisBinding.Z;

    [Header("Инверсия")]
    [SerializeField] private bool invertHorizontal = false;
    [SerializeField] private bool invertVertical = false;

    [Header("Чувствительность")]
    [SerializeField] private float sensitivity = 1f;

    [Header("Мёртвые зоны (0..0.5)")]
    [SerializeField][Range(0f, 0.5f)] private float horizontalDeadZone = 0.1f;
    [SerializeField][Range(0f, 0.5f)] private float verticalDeadZone = 0.1f;

    [Header("Калибровка")]
    [SerializeField] private bool calibrateOnStart = true;
    [SerializeField] private KeyCode calibrateKey = KeyCode.C;
    [SerializeField] private UnityEvent onCalibrate;

    [Header("Отладка")]
    [SerializeField] private bool useAccelerometer = true;
    [SerializeField] private Vector2 debugOutput;

    private Vector3 calibrationOffset = Vector3.zero;
    private bool isCalibrated = false;

    private enum AxisBinding { X, Y, Z, NegX, NegY, NegZ }

    private void Start()
    {
        if (calibrateOnStart)
            Calibrate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(calibrateKey))
            Calibrate();

        if (Application.isPlaying && useAccelerometer)
            debugOutput = GetInput();
    }

    public void Calibrate()
    {
        if (!useAccelerometer) return;
        calibrationOffset = Input.acceleration;
        isCalibrated = true;
        onCalibrate?.Invoke();
        Debug.Log($"Акселерометр откалиброван: {calibrationOffset}");
    }

    public Vector2 GetInput()
    {
        if (!useAccelerometer)
            return Vector2.zero;

        Vector3 accel = Input.acceleration;
        if (isCalibrated)
            accel -= calibrationOffset;

        float rawHorizontal = GetAxisValue(accel, horizontalAxis);
        float rawVertical = GetAxisValue(accel, verticalAxis);

        if (invertHorizontal) rawHorizontal = -rawHorizontal;
        if (invertVertical) rawVertical = -rawVertical;

        rawHorizontal *= sensitivity;
        rawVertical *= sensitivity;

        // Раздельная мёртвая зона
        if (Mathf.Abs(rawHorizontal) < horizontalDeadZone) rawHorizontal = 0f;
        if (Mathf.Abs(rawVertical) < verticalDeadZone) rawVertical = 0f;

        rawHorizontal = Mathf.Clamp(rawHorizontal, -1f, 1f);
        rawVertical = Mathf.Clamp(rawVertical, -1f, 1f);

        return new Vector2(rawHorizontal, rawVertical);
    }

    private float GetAxisValue(Vector3 v, AxisBinding axis)
    {
        switch (axis)
        {
            case AxisBinding.X: return v.x;
            case AxisBinding.Y: return v.y;
            case AxisBinding.Z: return v.z;
            case AxisBinding.NegX: return -v.x;
            case AxisBinding.NegY: return -v.y;
            case AxisBinding.NegZ: return -v.z;
            default: return 0f;
        }
    }

    public void ResetCalibration()
    {
        calibrationOffset = Vector3.zero;
        isCalibrated = false;
    }
}