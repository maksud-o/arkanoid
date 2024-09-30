using Arkanoid.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkanoid.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Board : MonoBehaviour
    {
        #region Variables

        [SerializeField] private InputActionReference _moveBoardPointerReference;
        [SerializeField] private GameObject _ball;

        private Camera _camera;
        private BoxCollider2D _collider;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (GamePrefsService.Instance.IsAutoPlay)
            {
                MoveAlongBallPosition();
            }
            else
            {
                MoveAlongPointerPosition();
            }
            ClampXWithinScreen();
        }

        #endregion

        #region Private methods

        private void ClampXWithinScreen()
        {
            float min = _camera.ScreenToWorldPoint(Vector2.zero).x + _collider.size.x / 2;
            float max = _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - _collider.size.x / 2;
            float x = transform.position.x;
            x = Mathf.Clamp(x, min, max);
            transform.position = new Vector2(x, transform.position.y);
        }

        private void MoveAlongPointerPosition()
        {
            Vector2 mousePosition = new(_moveBoardPointerReference.action.ReadValue<float>(), 0);
            Vector2 newPosition = new(_camera.ScreenToWorldPoint(mousePosition).x, transform.position.y);
            transform.position = newPosition;
        }

        private void MoveAlongBallPosition()
        {
            Vector2 newPosition = transform.position;
            newPosition.x = _ball.transform.position.x; 
            transform.position = newPosition;
        }

        #endregion
    }
}