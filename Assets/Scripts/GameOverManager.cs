using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void OnEnable()
    {
        Ball.OnFall += OnGameOver;
    }

    private void OnDisable()
    {
        Ball.OnFall -= OnGameOver;
    }

    private void OnGameOver()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
