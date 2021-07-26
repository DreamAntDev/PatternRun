using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Patten.Ads
{
    public class BannerAds : MonoBehaviour
    {
        [SerializeField]
        private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
        private const string surfacingId = "banner";

        private Coroutine _coroutine;

        void Start()
        {
            Advertisement.Initialize(Config.GameId, Config.IsTestAds);
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            if (Advertisement.Banner.isLoaded)
            {
                Advertisement.Banner.Hide();
            }
        }

        public void Show()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(ShowBannerWhenInitialized());
        }

        IEnumerator ShowBannerWhenInitialized()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (!Advertisement.isInitialized)
            {
                yield return wait;
            }
            Advertisement.Banner.SetPosition(_bannerPosition);
            Advertisement.Banner.Show(surfacingId);
        }
    }
}
