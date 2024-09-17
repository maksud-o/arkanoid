using System;
using UnityEngine;

namespace Arkanoid.Services
{
    public class PlayerStatsService : SingletonMonoBehaviour<PlayerStatsService>
    {
        #region Variables

        [SerializeField] private int _startingLives = 3; // Так можно?

        #endregion

        #region Events

        public event Action OnLivesChange;

        #endregion

        #region Properties

        public int Lives { get; private set; }

        #endregion

        #region Unity lifecycle

        private new void Awake()
        {
            base.Awake();
            Lives = _startingLives;
        }

        private void Start()
        {
            ScenesService.Instance.OnSceneReset += OnSceneResetCallBack;
        }

        #endregion

        #region Public methods

        public void GiveLives(int amount)
        {
            if (amount == 0)
            {
                return;
            }

            Lives += amount;
            OnLivesChange?.Invoke();
        }

        #endregion

        #region Private methods

        private void OnSceneResetCallBack()
        {
            Lives = _startingLives;
        }

        #endregion
    }
}