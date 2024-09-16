using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        #region Variables

        [SerializeField] private string _ballTag;

        [Header("Block Settings")]
        [Tooltip("Can be null")] [SerializeField]
        private List<Sprite> _multipleHitsSprites;
        [SerializeField] private int _scoreGiven = 1;
        private int _hitStage = 0;

        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Events

        public static event Action<int> OnBlockDestroy;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_ballTag))
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
        }

        #endregion

        #region Private methods

        private bool IsAlive()
        {
            return _hitStage < _multipleHitsSprites.Count;
        }

        private void HandleMultiHit()
        {
            _spriteRenderer.sprite = _multipleHitsSprites[_hitStage++];
        }

        #endregion
    }
}