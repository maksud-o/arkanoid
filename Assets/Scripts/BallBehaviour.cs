using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float _launchForce;
    [Header("Input Actions")]
    [SerializeField] private InputActionReference _launchBallReference;

    private Rigidbody2D _rb;

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
        gameObject.transform.parent = null;
        _rb.simulated = true;
        _rb.AddForce(Vector2.up * _launchForce, ForceMode2D.Impulse);
    }
}
