using Arkanoid.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _buttonText;
        private string _levelSceneName;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(LoadLevel);
        }

        #endregion

        #region Public methods

        public void Setup(string levelSceneName, int levelIndex)
        {
            _buttonText.text = $"{levelIndex}";
            _levelSceneName = levelSceneName;
        }

        #endregion

        #region Private methods

        private void LoadLevel()
        {
            ScenesService.Instance.LoadScene(_levelSceneName);
        }

        #endregion
    }
}