using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.PickUps
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PickUp : MonoBehaviour
    {
        #region Variables

        [SerializeField] private AudioClip _pickUpSound;
        [SerializeField] private float _fallSpeed = 3f;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * _fallSpeed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(GameInfo.BoardTag))
            {
                PerformActions();
                AudioService.Instance.PlaySound(_pickUpSound);
            }
            else if (!other.gameObject.CompareTag(GameInfo.FallColliderTag))
            {
                return;
            }

            Destroy(gameObject);
        }

        #endregion

        #region Protected methods

        protected abstract void PerformActions();

        #endregion
    }
}