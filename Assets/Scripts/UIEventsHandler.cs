using TMPro;
using UnityEngine;

namespace Arkanoid
{
    public class UIEventsHandler : MonoBehaviour
    {
        #region Variables

        [Header("Score Settings")]
        [SerializeField] private string _scoreTemplate = "Score: {0}";
        [SerializeField] private TextMeshProUGUI _scoreText;

        private int _currentScore;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            ChangeScoreText(_scoreTemplate, _currentScore);
        }

        private void OnEnable()
        {
            Block.OnBlockDestroy += ChangeScore;
        }

        private void OnDisable()
        {
            Block.OnBlockDestroy -= ChangeScore;
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