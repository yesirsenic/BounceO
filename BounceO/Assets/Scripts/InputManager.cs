using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public float rotateSpeed = 180f;


    [SerializeField]
    private GameObject Circle;

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            float touchX = Mouse.current.position.ReadValue().x;
            float screenCenterX = Screen.width * 0.5f;

            float direction = touchX < screenCenterX ? 1f : -1f;

            Circle.transform.Rotate(0f, 0f, direction * rotateSpeed * Time.deltaTime);
        }
    }
}
