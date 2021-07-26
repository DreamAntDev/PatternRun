using Patten.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;
    [SerializeField] InterstitialAds interstitialAds;

    private void Awake()
    {
        instance = this;
    }

    public void AdsShow()
    {
        interstitialAds.Show();
    }

}
