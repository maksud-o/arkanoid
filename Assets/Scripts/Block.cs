using Arkanoid.PickUps;
using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        #region Variables

        [Header("Block Settings")]
        [Tooltip("Can be null")]
        [SerializeField] private Sprite[] _multipleHitsSprites;
        [SerializeField] private PickUp[] _pickUps;
        [Range(0.01f, 1f)]
        [SerializeField] private float _pickUpChance = 1f;
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
            ProcessCollision();
        }

        #endregion

        #region Public methods

        public void ForceCollision()
        {
            ProcessCollision();
        }

        #endregion

        #region Private methods

        private void HandleMultiHit()
        {
            _spriteRenderer.sprite = _multipleHitsSprites[_hitStage++];
        }

        private bool IsAlive()
        {
            return _hitStage < _multipleHitsSprites.Length;
        }

        private void ProcessCollision()
        {
            if (_spriteRenderer.enabled == false)
            {
                _spriteRenderer.enabled = true;
            }
            else if (!IsAlive())
            {
                PlayerStatsService.Instance.ChangeScore(_scoreGiven);
                gameObject.SetActive(false);
                ProcessPickUp();
            }
            else
            {
                HandleMultiHit();
            }
        }

        private void ProcessPickUp()
        {
            if (_pickUps.Length == 0)
            {
                return;
            }

            if (Random.Range(0.01f, 1f) <= _pickUpChance)
            {
                Instantiate(_pickUps[Random.Range(0, _pickUps.Length)], transform.position, Quaternion.identity);
            }
        }

        #endregion
    }
}