using System;
using UnityEngine.SceneManagement;

namespace Arkanoid.Services
{
    public class ScenesService : SingletonMonoBehaviour<ScenesService>
    {
        public event Action OnSceneReset;
        #region Public methods

        public void ResetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            OnSceneReset?.Invoke();
        }

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, mode);
        }

        #endregion
    }
}