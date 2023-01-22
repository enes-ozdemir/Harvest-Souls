using UnityEngine;

namespace _Scripts
{
    public class PlayerTownController : MonoBehaviour
    {
        #region Singleton

        public static PlayerTownController Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        #endregion
    }
}