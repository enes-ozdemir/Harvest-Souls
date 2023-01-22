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
            text.gameObject.SetActive(false);
            skillImage.gameObject.SetActive(true);
            cooldownImage.fillAmount = 0;
            skillImage.sprite = sprite;
            this.keyCode = keyCode;
        }
        public void Remove()
        {
            skillImage.gameObject.SetActive(false);
            cooldownImage.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }

        public void SetCooldown(float amount, float targetFillAmount)
        {
            text.text = amount.ToString();
            cooldownImage.gameObject.SetActive(true);
            cooldownImage.fillAmount = 1;
            text.gameObject.SetActive(true);
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

            if (time >= duration)
            {
                text.gameObject.SetActive(false);
                cooldownImage.gameObject.SetActive(false);
            }

            image.fillAmount = end;
        }

        public void SetImage(Image image) => skillImage = image;
    }
}