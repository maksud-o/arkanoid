using System;
using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.Colliders
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class BallFallDetector : MonoBehaviour
    {
        #region Events

        public static event Action OnBallFall;

        #endregion

        #region Unity lifecycle

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(GameInfo.BallTag))
            {
                PlayerStatsService.Instance.ChangeLives(-1);
                if (PlayerStatsService.Instance.Lives <= 0)
                {
                    ScenesService.Instance.ResetScene();
                }

                OnBallFall?.Invoke();
            }
        }

        #endregion
    }
}