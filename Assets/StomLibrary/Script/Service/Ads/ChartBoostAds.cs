using UnityEngine;
using UnityEngine.Events;
using System.Collections;
#if CHARTBOOST
using ChartboostSDK;
#endif

namespace Stom.NativePlugin
{
    public class ChartBoostAds : MonoBehaviour
    {
        public void LoadInterstitial()
        {
#if CHARTBOOST
		Chartboost.cacheInterstitial(CBLocation.Default);
#endif
        }

        public void ShowInterstitial()
        {
#if CHARTBOOST
		Chartboost.showInterstitial(CBLocation.Default);
#endif
            LoadInterstitial();
        }

        public void LoadRewardVideo()
        {
#if CHARTBOOST
		Chartboost.cacheRewardedVideo(CBLocation.Default);
#endif
        }

        public void ShowRewardVideo()
        {
#if CHARTBOOST
		Chartboost.showRewardedVideo(CBLocation.Default);
#endif
            LoadRewardVideo();
        }

        public bool IsReady()
        {
#if CHARTBOOST
            return Chartboost.hasRewardedVideo (CBLocation.Default);
#else
            return false;
#endif
        }

#if ChartBoost
    void DidCompleteRewardedVideo(CBLocation location, int reward)
	{
			finishRewardVideo.Invoke ();
    EventManager.TriggerEvent(ServiceCallBackEvent.REWARD_SUCCESS);
	}
#endif
    }
}
