using System.Collections.Generic;
using System.Linq;
using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.UI
{
    public class UILivesController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Transform _livesParent;
        [SerializeField] private GameObject _lifePrefab;

        private List<GameObject> _lives;

        #endregion

        #region Unity lifecycle

        private void Start()
        {
            PlayerStatsService.Instance.OnLivesAdded += OnLivesAddedCallback;  
            PlayerStatsService.Instance.OnLivesRemoved += OnLivesRemovedCallback;

            _lives = new List<GameObject>();
            for (var i = 0; i < PlayerStatsService.Instance.MaxLives; i++)
            {
                _lives.Add(Instantiate(_lifePrefab, _livesParent));
            }

            int livesToRemove = PlayerStatsService.Instance.MaxLives - PlayerStatsService.Instance.Lives;
            for (var i = 0; i < livesToRemove; i++)
            {
                _lives[i].SetActive(false);
            }
        }

        private void OnDisable()
        {
            PlayerStatsService.Instance.OnLivesAdded -= OnLivesAddedCallback;
            PlayerStatsService.Instance.OnLivesRemoved -= OnLivesRemovedCallback;
        }

        #endregion

        #region Private methods

        private void OnLivesAddedCallback(int amount)
        {
            GameObject[] inactiveLives = _lives.Where(l => !l.activeSelf).ToArray();
            if (inactiveLives.Length == 0)
            {
                return;
            }

            for (var i = 0; i < amount; i++)
            {
                inactiveLives[i].SetActive(true);
            }
        }

        private void OnLivesRemovedCallback(int amount)
        {
            
            GameObject[] activeLives = _lives.Where(l => l.activeSelf).ToArray();
            if (activeLives.Length == 0)
            {
                return;
            }

            for (var i = 0; i < amount; i++)
            {
                activeLives[i].SetActive(false);
            }
        }

        #endregion
    }
}