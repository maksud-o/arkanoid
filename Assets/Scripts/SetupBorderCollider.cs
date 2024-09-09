using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class SetupBorderCollider : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private Camera _camera;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
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
        edgeCollider.SetPoints(points);
    }
}
