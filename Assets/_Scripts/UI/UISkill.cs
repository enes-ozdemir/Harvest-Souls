using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [Serializable]
    public class UISkill : MonoBehaviour
    {
        public string skillName;
        public Image skillImage;
        public Image cooldownImage;
        public TextMeshProUGUI text;
        public KeyCode keyCode;

        public void ToggleCooldown() => cooldownImage.gameObject.SetActive(!cooldownImage.gameObject.activeInHierarchy);

        public void SetCooldown(float amount, float fillAmount)
        {
            Debug.Log("Set cooldown" + fillAmount);
            text.text = amount.ToString();
            skillImage.fillAmount = fillAmount;
        }

        public void SetImage(Image image) => skillImage = image;
    }
}