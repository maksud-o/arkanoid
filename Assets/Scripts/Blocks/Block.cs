using System.Collections.Generic;
using Arkanoid.Services;
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
        [SerializeField] private bool _isInvisible;

        private int _hitStage;
        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Unity lifecycle

        protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_isInvisible)
            {
                _spriteRenderer.enabled = false;
            }
        }

        protected void OnCollisionEnter2D()
        {
            if (_spriteRenderer.enabled == false)
            {
                _spriteRenderer.enabled = true;
            }
            else if (!IsAlive())
            {
                PlayerStatsService.Instance.ChangeScore(_scoreGiven);
                gameObject.SetActive(false);
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