using System;
using UnityEngine;

namespace Arkanoid
{
    public class GlobalSystems : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}