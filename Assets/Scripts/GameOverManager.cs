using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void OnEnable()
    {
        BallBehaviour.OnFall += OnGameOver;
    }

    private void OnDisable()
    {
        BallBehaviour.OnFall -= OnGameOver;
    }

    private void OnGameOver()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
