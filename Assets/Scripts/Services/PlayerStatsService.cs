using System;
using UnityEngine;

namespace Arkanoid.Services
{
    public class PlayerStatsService : SingletonMonoBehaviour<PlayerStatsService>
    {
        #region Variables

        [SerializeField] private int _startingLives = 3;
        [SerializeField] private int _maxLives = 5;

        #endregion

        #region Events

        public event Action<int> OnLivesAdded;
        public event Action<int> OnLivesRemoved;
        public event Action OnScoreChange;

        #endregion

        #region Properties

        public int Lives { get; private set; }
        public int MaxLives => _maxLives;
        public int Score { get; private set; } = 0;

        #endregion

        #region Unity lifecycle

        protected override void AwakeAddition()
        {
            Lives = _startingLives;
        }

        private void Start()
        {
            ScenesService.Instance.OnSceneReset += OnSceneResetCallBack;
        }

        #endregion

        #region Public methods

        public void ChangeLives(int amount)
        {
            Lives += amount;
            switch (amount)
            {
                case < 0:
                    OnLivesRemoved?.Invoke(Math.Abs(amount));
                    break;
                case > 0:
                    OnLivesAdded?.Invoke(amount);
                    break;
            }
        }

        public void ChangeScore(int score)
        {
            Score += score;
            OnScoreChange?.Invoke();
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