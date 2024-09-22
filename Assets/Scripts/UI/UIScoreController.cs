using Arkanoid.Services;
using TMPro;
using UnityEngine;

namespace Arkanoid.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIScoreController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private string _scoreTemplate = "Score: {0}";
        private TextMeshProUGUI _scoreText;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            PlayerStatsService.Instance.OnScoreChange += OnScoreChangeCallback;
        }

        private void OnDisable()
        {
            PlayerStatsService.Instance.OnScoreChange -= OnScoreChangeCallback;
        }

        #endregion

        #region Private methods

        private void OnScoreChangeCallback()
        {
            ChangeScoreText(_scoreTemplate, PlayerStatsService.Instance.Score);
        }

        private void ChangeScoreText(string template, int score)
        {
            _scoreText.text = string.Format(template, score);
        }

        #endregion
    }
}