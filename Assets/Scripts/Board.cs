using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    #region Variables

    [SerializeField] private InputActionReference _moveBoardPointerReference;

    private Camera _camera;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        MoveAlongPointerPosition();
    }

    #endregion

    #region Private methods

    private void MoveAlongPointerPosition()
    {
        Vector2 mousePosition = new Vector2(_moveBoardPointerReference.action.ReadValue<float>(), 0);
        Vector2 newPosition = new Vector2(_camera.ScreenToWorldPoint(mousePosition).x, transform.position.y);
        transform.position = newPosition;
    }

    #endregion
}