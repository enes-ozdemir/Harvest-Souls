using System;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        private void Start()
        {
            SoundManager.Instance.ChangeMasterVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
        }
    }
}