using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace Patten.Ads
{
    public class RewardedAds : MonoBehaviour, IUnityAdsListener
    {
        const string mySurfacingId = "rewardedVideo";

        [SerializeField] private Button _rewardButton;

        void Start()
        {
            _rewardButton.enabled = false;

            Advertisement.AddListener(this);
            Advertisement.Initialize(Config.GameId, Config.IsTestAds);
        }

        public void Show()
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady(mySurfacingId))
            {
                Advertisement.Show(mySurfacingId);
            }
            else
            {
                Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            }
        }

        // Implement IUnityAdsListener interface methods:
        public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
        {
            if (surfacingId == mySurfacingId)
            {
                Debug.Log("# OnUnityAdsDidFinish : " + surfacingId + " : " + showResult.ToString());
            }
        }

        public void OnUnityAdsReady(string surfacingId)
        {
            if (surfacingId == mySurfacingId)
            {
                _rewardButton.enabled = true;
                Debug.Log("# OnUnityAdsReady : " + surfacingId);
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log("# OnUnityAdsDidError : " + message);
        }

        public void OnUnityAdsDidStart(string surfacingId)
        {
            if (surfacingId == mySurfacingId)
            {
                Debug.Log("# OnUnityAdsDidStart : " + surfacingId);
            }
        }

        // When the object that subscribes to ad events is destroyed, remove the listener:
        public void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }
    }
}