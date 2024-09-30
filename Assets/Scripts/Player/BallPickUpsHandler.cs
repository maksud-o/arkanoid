using System;
using System.Collections;
using Arkanoid.PickUps;
using Arkanoid.Services;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Arkanoid.Player
{
    [RequireComponent(typeof(Ball))]
    public class BallPickUpsHandler : MonoBehaviour
    {
        #region Variables

        [Header("Resize Settings")]
        [SerializeField] private float _maxScaleMultiplier = 2f;
        [SerializeField] private float _minScaleMultiplier = 0.5f;
        [SerializeField] private float _scaleStep = 0.1f;
        [Header("Catch Settings")]
        [SerializeField] private int _catchesAmount = 3;
        [Header("Explode Settings")]
        [SerializeField] private float _explodeDuration = 10f;
        [SerializeField] private float _explosionRadius = 2f;
        [FormerlySerializedAs("_explosiveLayerMask")] [SerializeField]
        private LayerMask _explosivesLayerMask;
        [SerializeField] private GameObject _explosionVFXPrefab;
        [SerializeField] private AudioClip _explosionAudioClip;

        private int _catchesCount;
        private float _currentScaleMultiplier = 1f;
        private bool _isCatchActive;
        private bool _isExplodeActive;
        private Vector3 _scaleProportions;

        #endregion

        #region Events

        public static event Action OnCatchActivated;
        public static event Action<Vector2> OnResizeActivated;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            BallResizePickUp.OnBallResizePickUp += BallResizePickUpCallback;
            BallCatchPickUp.OnBallCatchPickUp += BallCatchPickUpCallback;
            BallExplodePickUp.OnBallExplodePickUp += BallExplodePickUpCallback;

            _scaleProportions = transform.localScale;
        }

        private void OnDestroy()
        {
            BallResizePickUp.OnBallResizePickUp -= BallResizePickUpCallback;
            BallCatchPickUp.OnBallCatchPickUp -= BallCatchPickUpCallback;
            BallExplodePickUp.OnBallExplodePickUp -= BallExplodePickUpCallback;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(GameInfo.BoardTag) && _isCatchActive)
            {
                _catchesCount += 1;
                OnCatchActivated?.Invoke();
                if (_catchesCount > _catchesAmount)
                {
                    _isCatchActive = false;
                    _catchesCount = 0;
                }
            }

            ProcessExplosion();
        }

        #endregion

        #region Private methods

        private void BallCatchPickUpCallback()
        {
            _isCatchActive = true;
        }

        private void BallExplodePickUpCallback()
        {
            _isExplodeActive = true;
            StartCoroutine(ElapseTime(_explodeDuration));
        }

        private void BallMultiplyPickUpCallback() // todo: object pool
        {
            Debug.LogError("Not Implemented");
        }

        private void BallResizePickUpCallback()
        {
            bool isScaleDown = Random.value < 0.5f;
            if (isScaleDown && _currentScaleMultiplier - _scaleStep > _minScaleMultiplier)
            {
                _currentScaleMultiplier -= _scaleStep;
            }
            else if (!isScaleDown && _currentScaleMultiplier + _scaleStep < _maxScaleMultiplier)
            {
                _currentScaleMultiplier += _scaleStep;
            }

            OnResizeActivated?.Invoke(-_scaleProportions * _currentScaleMultiplier);
        }

        private IEnumerator ElapseTime(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isExplodeActive = false;
        }

        private void ProcessExplosion()
        {
            if (_isExplodeActive)
            {
                if (_explosionVFXPrefab == null)
                {
                    Debug.LogError($"No VFX Prefab assigned ({nameof(_explosionVFXPrefab)})");
                }

                if (_explosionAudioClip == null)
                {
                    Debug.LogError($"No VFX Prefab assigned ({nameof(_explosionAudioClip)})");
                }

                AudioService.Instance.PlaySound(_explosionAudioClip);

                Instantiate(_explosionVFXPrefab, transform.position, Quaternion.identity);

                Collider2D[] colliders =
                    Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _explosivesLayerMask);
                foreach (Collider2D col in colliders)
                {
                    Destroy(col.gameObject);
                }
            }
        }

        #endregion
    }
}