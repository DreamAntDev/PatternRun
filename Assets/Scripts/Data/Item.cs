using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NData
{
    [CreateAssetMenu(fileName="Item",menuName ="ScriptableObjects/Item",order = 1)]
    public class Item : ScriptableObject
    {
        public enum Type
        {
            Command,
            AutoActive,
        }
        public Type type;
        public List<string> usingCommand;
        public string iconName;
        public string patternName;
        public string textCode;
        public string actionName;
        public string triggerName;
        public string equipName;
        public int excuteCount;
        public bool stackable;
        public List<int> trapCode;
    }
}