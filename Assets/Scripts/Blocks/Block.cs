using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Blocks
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        #region Variables

        [Header("Block Settings")]
        [Tooltip("Can be null")]
        [SerializeField] private List<Sprite> _multipleHitsSprites;
        [SerializeField] private int _scoreGiven = 1;

        private int _hitStage;
        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Events

        public static event Action<int> OnBlockDestroy;

        #endregion

        #region Properties

        protected SpriteRenderer SpriteRenderer => _spriteRenderer;

        #endregion

        #region Unity lifecycle

        protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected void OnCollisionEnter2D()
        {
            if (!IsAlive())
            {
                gameObject.SetActive(false);
                OnBlockDestroy?.Invoke(_scoreGiven);
            }
            else
            {
                HandleMultiHit();
            }
        }

        #endregion

        #region Private methods

        private void HandleMultiHit()
        {
            _spriteRenderer.sprite = _multipleHitsSprites[_hitStage++];
        }

        private bool IsAlive()
        {
            return _hitStage < _multipleHitsSprites.Count;
        }

        #endregion
    }
}