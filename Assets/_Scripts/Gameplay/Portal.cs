using System;
using _Scripts.Data;
using _Scripts.Player;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Scripts.Gameplay
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private int battleLevel;
        [SerializeField] private int unlockCost;
        [SerializeField] private bool isUnlocked;
        [SerializeField] private Transform valueImage;
        [SerializeField] private GameObject dangerGameObject;

        private void Start()
        {
            text.gameObject.SetActive(false);
            errorText.gameObject.SetActive(false);
            valueImage.gameObject.SetActive(false);
            //if (PlayerStats.currentLevel >= battleLevel) _isUnlocked = true;
            if(!isUnlocked) dangerGameObject.SetActive(true);
        }

        private void UnlockPortal()
        {
            if (PlayerStats.soulAmount >= unlockCost)
            {
                PlayerStats.soulAmount -= unlockCost;
                isUnlocked = true;
            }
            else
            {
                ShowErrorMessage();
            }
        }

        private async UniTaskVoid ShowErrorMessage()
        {
            errorText.gameObject.SetActive(true);
            Camera.main.DOShakePosition(1f, fadeOut: true);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            errorText.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 15f)
            {
                text.gameObject.SetActive(true);
                valueImage.gameObject.SetActive(true);

                CheckIfPressed();
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }

        private void CheckIfPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isUnlocked)
                {
                    SceneManager.LoadScene("GameScene");
                }
                else
                {
                    UnlockPortal();
                }
            }
            
        }
    }
}