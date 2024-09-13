using UnityEngine.SceneManagement;

namespace Arkanoid.Services
{
    public class SceneLoadService : SingletonMonoBehaviour<SceneLoadService>
    {
        #region Public methods

        public void ResetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        #endregion
    }
}