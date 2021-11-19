using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Vector2 GetMouseWorldPosition()
    {
        return GameManager.Instance.MainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}