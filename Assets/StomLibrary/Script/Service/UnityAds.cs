using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;

namespace Stom.NativePlugin
{
    public class UnityAds : MonoBehaviour
    {
        public bool IsReady()
        {
#if UNITYADS && UNITY_ADS
        return Advertisement.IsReady ("rewardedVideo");
#else
            return false;
#endif
        }

        public void ShowRewardVideo()
        {
#if UNITYADS && UNITY_ADS
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
#endif
        }

        public void ShowAd()
        {
#if UNITYADS && UNITY_ADS
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
#endif
        }

#if UNITYADS && UNITY_ADS
	private void HandleShowResult(ShowResult result)
	{
            switch (result)
            {
                case ShowResult.Finished:
                    EventManager.TriggerEvent(ServiceCallBackEvent.REWARD_SUCCESS);
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }
#endif

    }
}
