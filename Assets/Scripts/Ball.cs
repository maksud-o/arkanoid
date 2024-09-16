using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Arkanoid
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _board;

        [Header("Ball Settings")]
        [SerializeField] private float _launchForce = 15;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference _launchBallReference;
        private bool _isLaunched;

        private Rigidbody2D _rb;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.simulated = false;
        }

        private void Update()
        {
            if (!_isLaunched)
            {
                MoveAlongBoard();
            }
        }

        private void OnEnable()
        {
            _launchBallReference.action.performed += OnLaunch;
        }

        private void OnDisable()
        {
            _launchBallReference.action.performed -= OnLaunch;
        }

        #endregion

        #region Private methods

        private void MoveAlongBoard()
        {
            transform.position = new Vector2(_board.transform.position.x, transform.position.y);
        }

        private void OnLaunch(InputAction.CallbackContext _)
        {
            if (!_isLaunched)
            {
                _isLaunched = true;
                _rb.simulated = true;
                _rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f).normalized * _launchForce,
                    ForceMode2D.Impulse);
            }
        }

        #endregion
    }
}