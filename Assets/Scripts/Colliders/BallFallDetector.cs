using System;
using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.Colliders
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class BallFallDetector : MonoBehaviour
    {
        public static event Action OnBallFall;
        
        #region Unity lifecycle

        private void OnCollisionEnter2D()
        {
            PlayerStatsService stats = PlayerStatsService.Instance;
            stats.GiveLives(-1);
            if (stats.Lives <= 0)
            {
                ScenesService.Instance.ResetScene();
            }
            OnBallFall?.Invoke();
        }

        #endregion
    }
}