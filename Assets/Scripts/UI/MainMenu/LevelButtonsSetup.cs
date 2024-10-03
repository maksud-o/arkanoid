using UnityEngine;

namespace Arkanoid.UI.MainMenu
{
    public class LevelButtonsSetup : MonoBehaviour
    {
        #region Variables

        [SerializeField] private string[] _levelScenesNames;
        [SerializeField] private LevelButton _levelButtonPrefab;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            var index = 1;
            foreach (string levelScenesName in _levelScenesNames)
            {
                Instantiate(_levelButtonPrefab.gameObject, transform).GetComponent<LevelButton>()
                    .Setup(levelScenesName, index++);
            }
        }

        #endregion
    }
}