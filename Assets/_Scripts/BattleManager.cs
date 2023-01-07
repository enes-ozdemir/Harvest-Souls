using System;
using UnityEngine;

namespace _Scripts
{
    public class BattleManager : MonoBehaviour
    {
        public static Action onBattleStarted;
        public static Action onBattleEnded;
        

        public void Start()
        {
            onBattleStarted.Invoke();
        }


        private void OnDisable()
        {
            onBattleEnded.Invoke();
        }
    }
}