using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class BorderColliderSetup : MonoBehaviour
{
    #region Variables

    [SerializeField] private float _angularOffset = 0.05f;
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
            _camera.ScreenToWorldPoint(new Vector2(0, Screen.height)),
            _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)),
            _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)),
        };
        // Adding angular offset to prevent ball from getting straight angle direction
        points[0] = new Vector2(points[0].x, points[0].y); 
        points[1] = new Vector2(points[1].x - _angularOffset, points[1].y + _angularOffset);
        points[2] = new Vector2(points[2].x - _angularOffset, points[2].y);
        points[3] = new Vector2(points[3].x, points[3].y + _angularOffset);

        _edgeCollider.SetPoints(points);
    }

    #endregion
}