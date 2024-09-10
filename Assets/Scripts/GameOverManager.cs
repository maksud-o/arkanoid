using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _ball;

    [Header("Game Over Settings")]
    [SerializeField] private float _heightThreshold = -8;

    private Scene _scene;

    private void Start()
    {
        _scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        GameOverCheck();
    }

    private void GameOverCheck()
    {
        if (_ball.transform.position.y <= _heightThreshold)
        {
            SceneManager.LoadScene(_scene.name, LoadSceneMode.Single);
        }
    }
}
