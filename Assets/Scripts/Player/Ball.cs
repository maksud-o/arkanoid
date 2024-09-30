using System;
using Arkanoid.Colliders;
using Arkanoid.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Arkanoid.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        #region Variables

        [SerializeField] private InputActionReference _launchBallInputActionReference;
        [SerializeField] private GameObject _board;
        [SerializeField] private AudioClip _ballCollisionSound;
        [SerializeField] private float _launchForce = 15;

        private bool _isLaunched;
        private Rigidbody2D _rb;

        private Vector2 _startingPosition;

        #endregion

        #region Unity lifecycle

        private void OnCollisionEnter2D(Collision2D other)
        {
            AudioService.Instance.PlaySound(_ballCollisionSound);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _launchBallInputActionReference.action.performed += OnLaunchBallPerformedCallback;

            BallFallDetector.OnBallFall += ResetBall;

            BallPickUpsHandler.OnCatchActivated += ResetBall;
            BallPickUpsHandler.OnResizeActivated += Resize;

            _rb.simulated = false;
            _startingPosition = transform.position;
        }

        private void Update()
        {
            if (!_isLaunched)
            {
                MoveAlongBoard();
            }

            if (!_isLaunched && GamePrefsService.Instance.IsAutoPlay)
            {
                Launch();
            }
        }

        private void OnDestroy()
        {
            _launchBallInputActionReference.action.performed -= OnLaunchBallPerformedCallback;
            BallFallDetector.OnBallFall -= ResetBall;
            BallPickUpsHandler.OnCatchActivated -= ResetBall;
            BallPickUpsHandler.OnResizeActivated -= Resize;
        }

        #endregion

        #region Private methods

        private void Launch()
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

        private void MoveAlongBoard()
        {
            transform.position = new Vector2(_board.transform.position.x, transform.position.y);
        }

        private void OnLaunchBallPerformedCallback(InputAction.CallbackContext context)
        {
            Launch();
        }

        private void ResetBall()
        {
            _rb.velocity = Vector2.zero;
            transform.position = _startingPosition;
            _isLaunched = false;
        }

        private void Resize(Vector2 size)
        {
            transform.localScale = size;
        }

        #endregion
    }
}