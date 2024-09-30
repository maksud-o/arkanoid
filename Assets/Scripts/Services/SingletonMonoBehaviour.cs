using UnityEngine;

namespace Arkanoid.Services
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Properties

        public static T Instance { get; private set; }

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            Instance = GetComponent<T>();
            AwakeAddition();
        }

        #endregion

        #region Protected methods

        protected virtual void AwakeAddition() { }

        #endregion
    }
}