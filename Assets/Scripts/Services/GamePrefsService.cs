using UnityEngine;

namespace Arkanoid.Services
{
    public class GamePrefsService : SingletonMonoBehaviour<GamePrefsService>
    {
        #region Variables

        [SerializeField] private bool _isAutoPlay;

        #endregion

        #region Properties

        public bool IsAutoPlay => _isAutoPlay;

        #endregion
    }
}