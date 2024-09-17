using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.Colliders
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class BallFallDetector : MonoBehaviour
    {
        #region Unity lifecycle

        private void OnCollisionEnter2D()
        {
            ScenesService.Instance.ResetScene();
        }

        #endregion
    }
}