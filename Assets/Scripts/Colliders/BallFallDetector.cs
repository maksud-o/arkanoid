using System;
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

        private void OnCollisionEnter2D()
        {
            OnBallFall?.Invoke();
        }

        #endregion
    }
}