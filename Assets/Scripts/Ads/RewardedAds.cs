using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace Patten.Ads
{
    public class RewardedAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
    {
        const string mySurfacingId = "rewardedVideo";

        [SerializeField] private Button _rewardButton;

        void Start()
        {
            _rewardButton.enabled = false;

            Advertisement.Initialize(Config.GameId, Config.IsTestAds);
        }
        
        private void AdsLoad()
        {
            Advertisement.Load(Config.RewardedVideoId,this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("OnInitializationComplete");
            Advertisement.Show(Config.RewardedVideoId, this);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogFormat("OnInitializationFailed : error: {0}, message: {1}", error, message);
        }
        
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            GameManager.instance.ContinueAdComplete();
        }
        
        //광고 show클릭되었을때
        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log("OnUnityAdsShowClick: " + placementId);
            AdsLoad();
        }
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("OnUnityAdsAdLoaded" + placementId);
        }
        
        //광고 로드 실패시
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log("OnUnityAdsFailedToLoaded" + placementId + error+message);
        }
        //광고 show 실패시
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogFormat("onunityAdsShowFailure: {0}, {1},{2}",placementId,error,message);
        }
        // 광고 show 시작시
        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("OnUnityAdsShowStart: " + placementId);
        }
    }
}