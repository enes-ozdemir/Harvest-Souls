using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Systems
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField] private List<string> lineList;
        [SerializeField] private GameObject dialogBox;
        [SerializeField] private TextMeshProUGUI dialogText;
        private bool _isSpeaking;
        
        private void Update()
        {
            if(_isSpeaking) return;;
            var playerPos = PlayerTownController.Instance.transform.position;
            CheckIfPlayerClose(playerPos);
        }

        private async UniTask CheckIfPlayerClose(Vector3 playerPos)
        {
            if (IsPlayerClose(playerPos))
            {
                await SaySomething();
                _isSpeaking = false;
            }
            else
            {
                dialogBox.SetActive(false);
            }
        }


        private bool IsPlayerClose(Vector3 playerPos)
        {
            return Vector2.Distance(playerPos, transform.position) < 8f;
        }

        private async UniTask SaySomething()
        {
            _isSpeaking = true;
            print("say");
            var randomIndex = Random.Range(0, lineList.Count);
            dialogBox.SetActive(true);
            dialogText.text = lineList[randomIndex];
            await UniTask.Delay(TimeSpan.FromSeconds(5f));

        }
    }
}