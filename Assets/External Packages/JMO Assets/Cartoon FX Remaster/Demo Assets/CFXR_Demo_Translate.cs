﻿//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace CartoonFX
{
    public class CFXR_Demo_Translate : MonoBehaviour
    {
        #region Variables

        public Vector3 direction = new(0, 1, 0);
        public bool randomRotation;

        private bool initialized;
        private Vector3 initialPosition;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                initialPosition = transform.position;
            }
        }

        private void Update()
        {
            transform.Translate(direction * Time.deltaTime);
        }

        private void OnEnable()
        {
            transform.position = initialPosition;
            if (randomRotation)
            {
                transform.eulerAngles = Vector3.Lerp(Vector3.zero, Vector3.up * 360, Random.value);
            }
        }

        #endregion
    }
}