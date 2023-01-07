using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class HealthUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;

        public void SetupHealthUI(int health)
        {
            healthText.text = $"x{health}";
        }
    }
}