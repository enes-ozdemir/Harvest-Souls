using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPositionList;

        #region Singleton

        public static MapManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        #endregion

        public Vector2 GetRandomPositionOnMap()
        {
            var randomIndex = Random.Range(0, spawnPositionList.Length - 1);
            return spawnPositionList[randomIndex].position;
        }
    }
}