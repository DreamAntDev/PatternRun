using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patten.Ads
{
    public static class Config
    {
        public static string GameId
        {
            get
            {
#if UNITY_EDITOR
                return "4233265";
#elif UNITY_ANDROID
                return "4233265";
#else // UNITY_IOS
                return "";
#endif
            }
        }

        public static bool IsTestAds
        {
            get
            {
                return true;
            }
        }
    }
}