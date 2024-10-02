using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkanoid
{
    public class PauseController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _pauseUI;
        [SerializeField] private InputActionReference _pauseActionReference;

        #endregion

        #region Events

        public static event Action OnPauseSwitch;

        #endregion

        #region Properties

        public static bool IsPaused { get; private set; } = false;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _pauseUI.SetActive(false);
            _pauseActionReference.action.performed += SwitchPause;
        }

        private void OnDestroy()
        {
            _pauseActionReference.action.performed -= SwitchPause;
        }

        #endregion

        #region Private methods

        private void SwitchPause(InputAction.CallbackContext _)
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1;
            _pauseUI.SetActive(!_pauseUI.activeSelf);
            OnPauseSwitch?.Invoke();
        }

        #endregion
    }
}