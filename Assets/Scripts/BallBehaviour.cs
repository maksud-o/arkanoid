using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float _launchForce = 20;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference _launchBallReference;

    private Rigidbody2D _rb;
    private bool _isLaunched = false;

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

    private void OnLaunch(InputAction.CallbackContext _)
    {
        _isLaunched = true;
        gameObject.transform.parent = null;
        _rb.simulated = true;
        _rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f).normalized * _launchForce, ForceMode2D.Impulse);
    }
}
