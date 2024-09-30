using System;

namespace Arkanoid.PickUps
{
    public class BallExplodePickUp : PickUp
    {
        #region Events

        public static event Action OnBallExplodePickUp;

        #endregion

        #region Protected methods

        protected override void PerformActions()
        {
            OnBallExplodePickUp?.Invoke();
        }

        #endregion
    }
}