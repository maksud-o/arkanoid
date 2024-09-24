using Arkanoid.Colliders;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkanoid.Services
{
    public class GameStateController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private InputActionReference _startGameInputActionReference;
        [SerializeField] private Ball _ball;

        #endregion

        private void Start()
        {
            _startGameInputActionReference.action.performed += StartGame;
            BallFallDetector.OnBallFall += ResetGame;
        }

        private void OnDestroy()
        {
            _startGameInputActionReference.action.performed -= StartGame;
            BallFallDetector.OnBallFall -= ResetGame;
        }

        #region Private methods

        private void ResetGame()
        {
            _ball.ResetBall();
            PlayerStatsService.Instance.ChangeLives(-1);
            if (PlayerStatsService.Instance.Lives <= 0)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            ScenesService.Instance.ResetScene();
        }

        private void StartGame(InputAction.CallbackContext _)
        {
            _ball.Launch();
        }

        #endregion
    }
}