using System.Net.Mime;
using Patten.Ads;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId;
    [SerializeField] string iOSGameId;
    [SerializeField] bool testMode = true;
    [SerializeField] bool enablePerPlacementMode = true;
    private string _GameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _GameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSGameId
            : androidGameId;
        Advertisement.Initialize(_GameId, testMode, new RewardedAds());
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}