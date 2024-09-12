using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private string _scoreTemplate = "Score: {0}";
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentScore = 0;

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

    private void ChangeScore(int score)
    {
        _currentScore += score;
        ChangeScoreText(_scoreTemplate, _currentScore);
    }

    private void ChangeScoreText(string template, int score)
    {
        _scoreText.text = string.Format(_scoreTemplate, score);
    }
}
