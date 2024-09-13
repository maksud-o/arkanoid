using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Colliders
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class BallFallColliderSetup : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float _heightThreshold = -8f;
        private Camera _camera;

        private EdgeCollider2D _edgeCollider;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _edgeCollider = GetComponent<EdgeCollider2D>();
        }

        private void Start()
        {
            _camera = Camera.main;
            SetupCollider();
        }

        #endregion

        #region Private methods

        private void SetupCollider()
        {
            List<Vector2> points = new()
            {
                _camera.ScreenToWorldPoint(Vector2.zero),
                _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)),
            };
            _edgeCollider.SetPoints(points);
            transform.position += new Vector3(0, 0, _heightThreshold);
        }

        #endregion
    }
}