using System;

namespace Arkanoid.PickUps
{
    public class BallResizePickUp : PickUp
    {
        #region Events

        public static event Action OnBallResizePickUp;

        #endregion

        #region Protected methods

        protected override void PerformActions()
        {
            OnBallResizePickUp?.Invoke();
        }

        #endregion
    }
}