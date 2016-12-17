using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Stom.NativePlugin;

public class Purchase : MonoBehaviour {
	public CoinIAPs consume;
	public ProductIAPs noneConsume;
	public Text nameCoin1;
	public Text priceCoin1;
	public Text nameCoin2;
	public Text priceCoin2;
	public Text priceRemoveAds;

	void Awake()
	{
		nameCoin1.text = "Get " + consume.coinIaps [0].coin.ToString () + " coins";
		nameCoin2.text = "Get " + consume.coinIaps [1].coin.ToString () + " coins";
		priceCoin1.text = "$" + consume.coinIaps [0].usd.ToString ();
		priceCoin2.text = "$" + consume.coinIaps [1].usd.ToString ();
		priceRemoveAds.text = "$" + noneConsume.productIaps [0].usd.ToString ();
		Stom.NativePlugin.EventManager.IapConsumeStartListening (PurchaseConsume);
		Stom.NativePlugin.EventManager.IapNoneConsumeStartListening (PurchaseNoneConsume);
	}

	void PurchaseConsume(int i)
	{
		GameManager.Instan.AddCoinTotal (consume.coinIaps [i].coin);
	}

	void PurchaseNoneConsume(int i)
	{
		if(i == 0)
		{
			PluginPersistent.Instance.Get_AdsController.DisableAds = true;
		}
	}
}
