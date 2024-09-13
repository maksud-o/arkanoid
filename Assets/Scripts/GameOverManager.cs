using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    #region Unity lifecycle

    private void OnEnable()
    {
        Ball.OnFall += OnGameOver;
    }

    private void OnDisable()
    {
        Ball.OnFall -= OnGameOver;
    }

    #endregion

    #region Private methods

    private void OnGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    #endregion
}