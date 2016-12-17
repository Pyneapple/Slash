using UnityEngine;
using System.Collections;
using Stom.NativePlugin;
using UnityEngine.UI;
using Stom;

public class IAPButton : ButtonRegister {

    private IAPurchase IAP { get { return PluginPersistent.Instance.Get_InAppPurchase; } }

    public void BuyProductConsume(int indexProduct)
    {
        IAP.BuyProductConsume(indexProduct);
    }

    public void BuyProductNonConsume(int indexPorduct)
    {
        IAP.BuyProductNonConsume(indexPorduct);
    }

    public void RestorePurchases()
    {
        IAP.RestorePurchases();
    }

    #region Editor apply button
#if UNITY_EDITOR
    public void RegistPurchaseButton(Button _button, int index, bool isConsume)
    {
        UnityEditor.EditorUtility.SetDirty(_button);
        while (_button.onClick.GetPersistentEventCount() != 0)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(_button.onClick, 0);
        if (isConsume)
            UnityEditor.Events.UnityEventTools.AddIntPersistentListener(_button.onClick, BuyProductConsume, index);
        else
            UnityEditor.Events.UnityEventTools.AddIntPersistentListener(_button.onClick, BuyProductNonConsume, index);
    }

    public void UnRegistPurchaseButton(Button _button,bool isConsume)
    {
        string _removeMethod = isConsume ? "BuyProductConsume" : "BuyProductNonConsume";
        for (int i = 0; i < _button.onClick.GetPersistentEventCount(); i++)
        {
            if (_button.onClick.GetPersistentMethodName(i) == _removeMethod)
                UnityEditor.Events.UnityEventTools.RemovePersistentListener(_button.onClick, 0);
        }
    }
#endif
    #endregion

    #region Overrite Function
    protected override void ButtonCall(string nameMethod)
    {
        ButtonCall<IAPButton>(nameMethod);
    }
    #endregion
}
