using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class BorderColliderSetup : MonoBehaviour
{
    private EdgeCollider2D _edgeCollider;
    private Camera _camera;

    private void Awake()
    {
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _camera = Camera.main;
    }

    private void Start()
    {
        var points = new List<Vector2>
        {
            _camera.ScreenToWorldPoint(Vector2.zero),
            _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)),
            _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)),
            _camera.ScreenToWorldPoint(new Vector2(0, Screen.height)),
            _camera.ScreenToWorldPoint(Vector2.zero)
        };
        _edgeCollider.SetPoints(points);
    }
}
