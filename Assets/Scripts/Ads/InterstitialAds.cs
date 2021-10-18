using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Patten.Ads
{
    public class InterstitialAds : MonoBehaviour, IUnityAdsListener
    {
        const string mySurfacingId = "interstitial";

        private Coroutine _coroutine;

        void Awake()
        {
            InitializeAds();
        }

        public void InitializeAds()
        {
            Advertisement.Initialize(Config.GameId, false);
            Advertisement.AddListener(this);
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            Advertisement.RemoveListener(this);
        }

        public void Show()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(ShowInterstitialWhenInitialized());
        }

        IEnumerator ShowInterstitialWhenInitialized()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while (!Advertisement.isInitialized || !Advertisement.IsReady())
            {
                yield return wait;
            }
            Advertisement.Show();
        }

        void IUnityAdsListener.OnUnityAdsReady(string placementId)
        {

        }

        void IUnityAdsListener.OnUnityAdsDidError(string message)
        {

        }

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
        {

        }

        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            GameManager.instance.ContinueAdComplete();
        }
    }
}