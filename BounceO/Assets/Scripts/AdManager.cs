using UnityEngine;
using GoogleMobileAds.Api;
using System;


public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private InterstitialAd interstitialAd;

    private const string INTERSTITIAL_ID =
        "ca-app-pub-3940256099942544/1033173712";

    public bool NoAds
    {
        get { return PlayerPrefs.GetInt("NO_ADS", 0) == 1; }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MobileAds.Initialize(_ =>
        {
            LoadInterstitial();
        });
    }

    void LoadInterstitial()
    {
        if (NoAds) return;

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var request = new AdRequest();

        InterstitialAd.Load(INTERSTITIAL_ID, request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial failed to load: " + error);
                    return;
                }

                interstitialAd = ad;

                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    LoadInterstitial();
                };

                interstitialAd.OnAdFullScreenContentFailed += err =>
                {
                    Debug.LogError("Interstitial failed to show: " + err);
                    LoadInterstitial();
                };
            });
    }

    public void ShowInterstitial()
    {
        if (NoAds)
        {
            Debug.Log("No Ads purchased. Skip ad.");
            return;
        }

        if (interstitialAd != null)
        {
            interstitialAd.Show();
        }

        else
        {
            LoadInterstitial();
        }
    }

}


