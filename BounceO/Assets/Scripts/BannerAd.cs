using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAd : MonoBehaviour
{
    public static BannerAd Instance;

    private BannerView bannerView;

#if UNITY_ANDROID
    private const string BANNER_ID = "ca-app-pub-9548284037151614/6682984808";
#elif UNITY_IOS
    private const string BANNER_ID = "ca-app-pub-XXXXXXXXXX/IIIIIIIIII";
#endif

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Show();
    }

    public void Show()
    {
        if (PlayerPrefs.GetInt("NO_ADS", 0) == 1)
            return;

        if (bannerView != null)
            return;

        bannerView = new BannerView(
            BANNER_ID,
            AdSize.Banner,
            AdPosition.Bottom
        );

        var request = new AdRequest();
        bannerView.LoadAd(request);
    }

    public void Hide()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
