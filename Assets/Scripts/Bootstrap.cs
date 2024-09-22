using System;
using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private string _startSceneName;

        private void Start()
        {
            ScenesService.Instance.LoadScene(_startSceneName);
        }
    }
}
