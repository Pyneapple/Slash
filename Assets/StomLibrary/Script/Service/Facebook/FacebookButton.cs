using UnityEngine;
using System.Collections;
using Stom.NativePlugin;
using UnityEngine.UI;
using Stom;

public class FacebookButton : ButtonRegister {

    private FacebookIntegrate FacebookIngr { get { return PluginPersistent.Instance.Get_FacebookIngr; } }

    public void Login()
    {
        FacebookIngr.Login();
    }

    public void LogOut()
    {
        FacebookIngr.LogOut();
    }

    public void ShareLink()
    {
        FacebookIngr.ShareLink();
    }

    public void FeedShare()
    {
        FacebookIngr.FeedShare();
    }

    public void AppInvite()
    {
        FacebookIngr.AppInvite();
    }

    #region Overrite Function
    protected override void ButtonCall(string nameMethod)
    {
        ButtonCall<FacebookButton>(nameMethod);
    }
    #endregion
}
