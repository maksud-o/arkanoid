using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveBoardPointerReference;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        MoveAlongPointerPosition();
    }

    private void MoveAlongPointerPosition()
    {
        var mousePosition = new Vector2(_moveBoardPointerReference.action.ReadValue<float>(), 0);
        var newPosition = new Vector2(_camera.ScreenToWorldPoint(mousePosition).x, transform.position.y);
        transform.position = newPosition;
    }
}
