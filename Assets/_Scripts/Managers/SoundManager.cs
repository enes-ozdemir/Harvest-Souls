using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        #region Singleton

        public static SoundManager Instance;

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            DontDestroyOnLoad(this);
        }

        #endregion

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource effectSource;


        public void PlaySound(AudioClip clip)
        {
            effectSource.PlayOneShot(clip);
        }

        public void ChangeMasterVolume(float value)
        {
            AudioListener.volume = value;
        }

        public void ToggleEffects() => effectSource.mute = !effectSource.mute;
        
        public void ToggleMusic() => musicSource.mute = !musicSource.mute;
    }
}