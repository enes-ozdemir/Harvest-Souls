using System;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Menu
{
    public class PlaySoundOnStart : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;

        private void Start()
        {
            SoundManager.Instance.PlaySound(clip);
        }
    }
}