using System;
using _Scripts.Data;
using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class TopUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI soulText;

        public Action<int> onHealthChanged;

        private void Start()
        {
            SoulManager.Instance.OnSoulCollected += SetupSoulUI;
            onHealthChanged += SetupHealthUI;
        }

        public void SetupHealthUI(int health)
        {
            healthText.text = $"x{health}";
        }

        private void SetupSoulUI(int amount)
        {
            soulText.text = $"x{PlayerStats.soulAmount}";
        }
    }
}