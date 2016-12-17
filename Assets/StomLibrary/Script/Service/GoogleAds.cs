using System;
using UnityEngine;
#if GOOGLE_ADS
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif
namespace Stom.NativePlugin
{
    [System.Serializable]
    public class NormalBanner
    {
        public string idBannerAndroid;
        public string idBannerIos;
#if GOOGLE_ADS
	public AdPosition positionShow;
	private BannerView bannerView;
#endif
        public void Init()
        {
            RequestBanner();
        }

        public void ShowBanner()
        {
#if GOOGLE_ADS
		bannerView.Show ();
#endif
        }

        public void HideBanner()
        {
#if GOOGLE_ADS
		bannerView.Hide ();
#endif
        }

        public void RequestBanner()
        {
#if UNITY_ANDROID
		string adUnitId = idBannerAndroid;
#elif UNITY_IPHONE
		string adUnitId = idBannerIos;
#else
		string adUnitId = "unexpected_platform";
#endif
#if GOOGLE_ADS
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, positionShow);
		// Register for ad events.
		bannerView.OnAdLoaded += HandleAdLoaded;
		// Load a banner ad.
		bannerView.LoadAd(createAdRequest());
#endif
        }
#if GOOGLE_ADS
	private AdRequest createAdRequest()
	{
	return new AdRequest.Builder()
	.AddTestDevice(AdRequest.TestDeviceSimulator)
	.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
	.AddKeyword("game")
	.SetGender(Gender.Male)
	.SetBirthday(new DateTime(1985, 1, 1))
	.TagForChildDirectedTreatment(false)
	.AddExtra("color_bg", "9B30FF")
	.Build();
	}


	public void HandleAdLoaded(object sender, EventArgs args)
	{
		HideBanner ();
	}
#endif
    }

    [System.Serializable]
    public class FullBanner
    {
        public string idBannerAndroid;
        public string idBannerIos;

#if GOOGLE_ADS
	private InterstitialAd interstitial;
	private float backupTimeScale;
#endif

        public void Init()
        {
            RequestInterstitial();
        }

        public void ShowFullBanner()
        {
#if GOOGLE_ADS
		if (interstitial.IsLoaded())
		{
		interstitial.Show();
		RequestInterstitial ();
        backupTimeScale = Time.timeScale;
        //Time.timeScale = 0.0f;
		}
#endif
        }

        public void RequestInterstitial()
        {
#if UNITY_ANDROID
	string adUnitId = idBannerAndroid;
#elif UNITY_IOS
	string adUnitId = idBannerIos;
#else
            string adUnitId = "unexpected_platform";
#endif
#if GOOGLE_ADS
            // Create an interstitial.
            interstitial = new InterstitialAd(adUnitId);
            // Load an interstitial ad.
            interstitial.LoadAd(createAdRequest());
//            interstitial.OnAdClosed += HandleInterstitialClosed;
#endif

        }
#if GOOGLE_ADS
        // Returns an ad request with custom ad targeting.
        private AdRequest createAdRequest()
        {
            return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();

        }
#endif

        /// <summary>
        /// Resume time scale
        /// </summary>
        private void HandleInterstitialClosed()
        {
#if GOOGLE_ADS
            Time.timeScale = backupTimeScale;
            //interstitial.OnAdClosed -= HandleInterstitialClosed;
#endif
        }
    }

    public class GoogleAds : MonoBehaviour
    {
        public NormalBanner banner;
        public FullBanner fullBanner;

        public void InitialBanner()     { banner.Init(); }
        public void InitialFullBanner() { fullBanner.Init(); }
    }
}
