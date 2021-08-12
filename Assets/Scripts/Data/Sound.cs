using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NData
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Sound", order = 1)]
    public class Sound : ScriptableObject
    {
        [System.Serializable]
        public class SoundPair
        {
            public SoundManager.SoundType type;
            public string path;
        }
        public List<SoundPair> soundList;
    }
}
