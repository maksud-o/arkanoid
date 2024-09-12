using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public static event Action OnFall;

    [Header("Ball Settings")]
    [SerializeField] private float _launchForce = 20;
    [SerializeField] private float _heightThreshold = -8f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference _launchBallReference;

    private Rigidbody2D _rb;
    private bool _isLaunched = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
    }

    private void Update()
    {
        if (transform.position.y <= _heightThreshold)
        {
            OnFall?.Invoke();
        }
    }

    private void OnEnable()
    {
        _launchBallReference.action.performed += OnLaunch;
    }

    private void OnDisable()
    {
        _launchBallReference.action.performed -= OnLaunch;
    }

    private void OnLaunch(InputAction.CallbackContext _)
    {
        if (!_isLaunched)
        {
            _isLaunched = true;
            gameObject.transform.parent = null;
            _rb.simulated = true;
            _rb.AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), 1f).normalized * _launchForce, ForceMode2D.Impulse);
        }
    }
}
