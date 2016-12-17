using UnityEngine;
using System.Collections;
using Stom.NativePlugin;
using UnityEngine.UI;
using Stom;

public class AdsButton : ButtonRegister {

    public void ShowRewardUnityAds() { ShowRewardVideo(); }

    public void ShowRewardChartboost() { ShowRewardVideo(); }

    private void ShowRewardVideo() { PluginPersistent.Instance.Get_AdsController.ShowRewardVideo(); }

    #region Overrite Function
    protected override void ButtonCall(string nameMethod)
    {
        ButtonCall<AdsButton>(nameMethod);
    }
    #endregion
}
