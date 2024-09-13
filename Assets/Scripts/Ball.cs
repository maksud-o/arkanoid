using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    #region Variables

    [Header("Ball Settings")]
    [SerializeField] private float _launchForce = 20;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference _launchBallReference;
    private bool _isLaunched;

    private Rigidbody2D _rb;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
    }

    private void OnEnable()
    {
        _launchBallReference.action.performed += OnLaunch;
    }

    private void OnDisable()
    {
        _launchBallReference.action.performed -= OnLaunch;
    }

    #endregion

    #region Private methods

    private void OnLaunch(InputAction.CallbackContext _)
    {
        if (!_isLaunched)
        {
            _isLaunched = true;
            gameObject.transform.parent = null;
            _rb.simulated = true;
            _rb.AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), 1f).normalized * _launchForce,
                ForceMode2D.Impulse);
        }
    }

    #endregion
}