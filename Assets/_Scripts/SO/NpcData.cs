using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu]
    public class NpcData : ScriptableObject
    {
        public List<string> lines;
        public Sprite[] idleSprites;
    }
}