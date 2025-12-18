using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public float rotateSpeed = 180f;

    private float currentAngle = 0f;


    [SerializeField]
    private GameObject Circle;

    private void Update()
    {
        if(EventSystem.current != null)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                if (EventSystem.current.IsPointerOverGameObject(
                    Touchscreen.current.primaryTouch.touchId.ReadValue()))
                    return;
            }

            else if(Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
            }
        }

        if(Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            float touchX = Touchscreen.current.primaryTouch.position.ReadValue().x;
            RotateByPosition(touchX);
        }

        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            float mouseX = Mouse.current.position.ReadValue().x;
            RotateByPosition(mouseX);
        }

        Circle.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

    }

    private void RotateByPosition(float x)
    {
        float screenCenterX = Screen.width * 0.5f;
        float direction = x < screenCenterX ? 1f : -1f;

        currentAngle += direction * rotateSpeed * Time.deltaTime;
    }
}
