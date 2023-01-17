using _Scripts.Data;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers
{
    public class TownUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI soulText;

        private void Start()
        {
            SetSoulUI();
        }

        private void SetSoulUI()
        {
            soulText.text = $"x{PlayerStats.soulAmount}";
        }
    }
}