using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float _launchForce = 20; // Хорошая ли идея для игровых настроек в этом и других скриптах создать один ScriptableObject и прокидывать его везде где нужно?

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
        _rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f) * _launchForce, ForceMode2D.Impulse);
    }
}
