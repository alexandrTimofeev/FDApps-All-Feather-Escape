using UnityEngine;

public class JoystickInputProvider : MonoBehaviour, IInputProvider
{
    [SerializeField] private Joystick joystick; 

    public Vector2 GetInput()
    {
        return new Vector2(joystick.Horizontal, joystick.Vertical);
    }
}
