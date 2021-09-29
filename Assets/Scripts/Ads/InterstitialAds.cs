using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Patten.Ads
{
    public class InterstitialAds : MonoBehaviour
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
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
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
    }
}