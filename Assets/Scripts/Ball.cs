using UnityEngine;
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

        private bool _isLaunched;
        private Rigidbody2D _rb;

        private Vector2 _startingPosition;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.simulated = false;
            _startingPosition = transform.position;
        }

        private void Update()
        {
            if (!_isLaunched)
            {
                MoveAlongBoard();
            }
        }

        #endregion

        #region Public methods

        public void Launch()
        {
            if (_isLaunched)
            {
                return;
            }

            _isLaunched = true;
            _rb.simulated = true;
            _rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f).normalized * _launchForce,
                ForceMode2D.Impulse);
        }

        public void ResetBall()
        {
            _rb.velocity = Vector2.zero;
            transform.position = _startingPosition;
            _isLaunched = false;
        }

        #endregion

        #region Private methods

        private void MoveAlongBoard()
        {
            transform.position = new Vector2(_board.transform.position.x, transform.position.y);
        }

        #endregion
    }
}