using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace NData
{
    [CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Language", order = 1)]
    public class Language : ScriptableObject
    {
        public enum Local
        {
            Korea,
            English,
        }
        
        [System.Serializable]
        public class LanguagePair
        {
            public string key;
            public List<LocalLanguagePair> languageDictionary = new List<LocalLanguagePair>();
        }

        [System.Serializable]
        public class LocalLanguagePair
        {
            public Local local;
            public string text;
        }


        public List<LanguagePair> LanguagePairList = new List<LanguagePair>();

        public string GetLanguage(string key)
        {
            var findObj = this.LanguagePairList.Find(o => o.key.Equals(key));
            if (findObj == null)
                return key;

            var localObj = findObj.languageDictionary.Find(o => o.local.Equals(Local.English));
            if (localObj == null)
                return key;

            return localObj.text;
        }
    }
}
