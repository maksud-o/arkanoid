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

        public void AddLives(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError($"Invalid amount in {nameof(AddLives)}");
            }

            Lives += amount;
            OnLivesAdded?.Invoke(amount);
        }

        public void RemoveLives(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError($"Invalid amount in {nameof(RemoveLives)}");
            }
            
            Lives += amount;
            OnLivesRemoved?.Invoke(amount);
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