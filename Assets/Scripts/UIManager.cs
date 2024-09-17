using System;
using System.Collections.Generic;
using Arkanoid.Blocks;
using Arkanoid.Services;
using TMPro;
using UnityEngine;

namespace Arkanoid
{
    public class UIManager : MonoBehaviour
    {
        #region Variables

        [Header("Score")]
        [SerializeField] private string _scoreTemplate = "Score: {0}";
        [SerializeField] private TextMeshProUGUI _scoreText;
        [Header("Lives")]
        [SerializeField] private Transform _livesParent;
        [SerializeField] private GameObject _lifePrefab;

        private int _currentScore;
        private List<GameObject> _lives;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            ChangeScoreText(_scoreTemplate, _currentScore);
        }

        private void Start()
        {
            Block.OnBlockDestroy += ChangeScore;
            PlayerStatsService.Instance.OnLivesChange += OnLivesChangeCallback;
            
            _lives = new List<GameObject>();
            for (int i = 0; i < PlayerStatsService.Instance.Lives; i++)
            {
                _lives.Add(Instantiate(_lifePrefab, _livesParent));
            }
        }

        private void OnLivesChangeCallback()
        {
            int amount = PlayerStatsService.Instance.Lives - _lives.Count;
            if (amount < 0)
            {
                amount *= -1;
                for (int i = 0; i < amount; i++)
                {
                    _lives[i].SetActive(false);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void OnDisable()
        {
            Block.OnBlockDestroy -= ChangeScore;
            PlayerStatsService.Instance.OnLivesChange -= OnLivesChangeCallback;
        }

        #endregion

        #region Private methods

        private void ChangeScore(int score)
        {
            _currentScore += score;
            ChangeScoreText(_scoreTemplate, _currentScore);
        }

        private void ChangeScoreText(string template, int score)
        {
            _scoreText.text = string.Format(_scoreTemplate, score);
        }

        #endregion
    }
}