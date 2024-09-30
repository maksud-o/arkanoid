using System;
using UnityEngine.SceneManagement;

namespace Arkanoid.Services
{
    public class ScenesService : SingletonMonoBehaviour<ScenesService>
    {
        #region Events

        public event Action OnSceneReset;

        #endregion

        #region Public methods

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, mode);
        }

        public void ResetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            OnSceneReset?.Invoke();
        }

        #endregion
    }
}