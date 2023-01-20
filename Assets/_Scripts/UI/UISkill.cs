using System;
using System.Collections;
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

        public void Setup(Sprite sprite, KeyCode keyCode)
        {
            print("Setup UISkill");
            skillImage.sprite = sprite;
            this.keyCode = keyCode;
        }

        public void ToggleCooldown() => cooldownImage.gameObject.SetActive(!cooldownImage.gameObject.activeInHierarchy);

        public void SetCooldown(float amount, float targetFillAmount)
        {
            text.text = amount.ToString();
            text.gameObject.SetActive(true);
            cooldownImage.fillAmount = 1;
            StartCoroutine(LerpFillAmount(cooldownImage, cooldownImage.fillAmount, targetFillAmount, 0.5f));
        }

        private IEnumerator LerpFillAmount(Image image, float start, float end, float duration)
        {
            float time = 0;
            while (time <= duration)
            {
                float fillAmount = Mathf.Lerp(start, end, time / duration);
                image.fillAmount = fillAmount;
                yield return null;
                time += Time.deltaTime;
            }

            image.fillAmount = end;
            if (duration <= 0.1f)
            {
                text.gameObject.SetActive(false);
                ToggleCooldown();
            }
        }

        public void SetImage(Image image) => skillImage = image;
    }
}