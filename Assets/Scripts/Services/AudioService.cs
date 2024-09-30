using UnityEngine;

namespace Arkanoid.Services
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioService : SingletonMonoBehaviour<AudioService>
    {
        #region Variables

        private AudioSource _audioSource;

        #endregion

        #region Public methods

        public void PlaySound(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogError($"Clip is null ({nameof(PlaySound)})");
                return;
            }

            _audioSource.PlayOneShot(clip);
        }

        protected override void AwakeAddition()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion
    }
}