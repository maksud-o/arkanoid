using System;
using UnityEngine;

namespace Arkanoid.PickUps
{
    public class BallCatchPickUp : PickUp
    {
        public static event Action OnBallCatchPickUp;
        
        protected override void PerformActions()
        {
            OnBallCatchPickUp?.Invoke();
        }
    }
}