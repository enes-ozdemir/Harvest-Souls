using UnityEngine;

namespace _Scripts.Gameplay
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private float boundsX;
        [SerializeField] private float boundsY;

        public static float mapWidth;
        public static float mapHeight;

        private void Awake()
        {
            mapWidth = boundsX;
            mapHeight = boundsY;
        }

        public static Vector2 GetRandomPositionOnMap()
        {
            var newPath = new Vector2(Random.Range(-mapWidth, mapWidth),
                Random.Range(-mapHeight, mapHeight));
//            Debug.Log("NEW PATH set" + newPath);
            return newPath;
        }
    }
}