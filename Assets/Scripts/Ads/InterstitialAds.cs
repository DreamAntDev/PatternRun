using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Patten.Ads
{
    public class InterstitialAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
    {
        public void InitializeAds()
        {
            Advertisement.Initialize(Config.GameId, true);
        }

        public void AdsShow()
        {
            InitializeAds();
            Advertisement.Show(Config.RewardedVideoId, this);
        }

        private void AdsLoad()
        {
            Advertisement.Load(Config.RewardedVideoId,this);
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("# OnInitializationComplete");
            AdsShow();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogFormat("# OnInitializationFailed : error: {0}, message: {1}", error, message);
        }
        
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            GameManager.instance.ContinueAdComplete();
        }
        
        //광고 show클릭되었을때
        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log("# OnUnityAdsShowClick: " + placementId);
            AdsLoad();
        }
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("# OnUnityAdsAdLoaded" + placementId);
        }
        
        //광고 로드 실패시
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log("# OnUnityAdsFailedToLoaded" + placementId + error+message);
        }
        //광고 show 실패시
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogFormat("# OnunityAdsShowFailure: {0}, {1},{2}",placementId,error,message);
        }
        // 광고 show 시작시
        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("# OnUnityAdsShowStart: " + placementId);
        }
    }
}