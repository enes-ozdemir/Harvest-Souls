using _Scripts.Data;
using _Scripts.Player;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers
{
    public class TopUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI soulText;

        private void Start()
        {
            SoulManager.Instance.OnSoulCollected += SetupSoulUI;
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