using _Scripts.SO;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu]
    public class GameData : ScriptableObject
    {
        public PlayerData playerData;
        public int soulCount;
    }
}