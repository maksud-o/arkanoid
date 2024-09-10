using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private string _scoreTemplate = "Score: ";
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentScore = 0;

    private void Awake()
    {
        _scoreText.text = $"{_scoreTemplate}{_currentScore}";
    }

    private void OnEnable()
    {
        BlockBehaviour.OnBlockDestroy += ChangeScore;
    }

    private void OnDisable()
    {
        BlockBehaviour.OnBlockDestroy -= ChangeScore;
    }

    private void ChangeScore(int score)
    {
        _currentScore += score;
        _scoreText.text = $"{_scoreTemplate}{_currentScore}";
    }
}
