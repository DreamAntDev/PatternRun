using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NData
{
    [CreateAssetMenu(fileName="Item",menuName ="ScriptableObjects/Item",order = 1)]
    public class Item : ScriptableObject
    {
        public List<string> usingCommand;
        public string iconName;
        public string patternName;
        public string actionName;
    }
}