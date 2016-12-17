using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaceShop : MonoBehaviour {
	public static int plaseActive;
	public DataShop data;
	public AvataCharacter avataCharacter;
	public Text nameProduct;
	public Text description;
	public Text coinText;
	public GameObject buy;
	public GameObject active;
	public Button buttonActive;
	public bool purchased
	{
		get{
			return PlayerPrefs.HasKey ("place" + transform.GetSiblingIndex ().ToString ());
		}
		set
		{
			if(value)
			{
				PlayerPrefs.SetInt ("place" + transform.GetSiblingIndex ().ToString (), 1);
			}
		}
	}

	void Awake()
	{
		if(transform.GetSiblingIndex () == 0)
		{
			if(PlayerPrefs.HasKey ("PlaceActive"))
			{
				plaseActive = PlayerPrefs.GetInt ("PlaceActive");
			}
			else
			{
				plaseActive = 0;
			}
		}
		if(data.purchased)
		{
			purchased = true;
		}
	}

	void OnEnable()
	{
		SetupCharacter ();
	}

	void Start()
	{
		SetupCharacter ();
	}

	public void SetupCharacter()
	{
		nameProduct.text = data.nameProduc;
		description.text = data.description;
		coinText.text = data.coin.ToString ();

		if(purchased)
		{
			CheckBuy (true);
			if(plaseActive == transform.GetSiblingIndex ())
			{
				Active ();
			}
			else
			{
				DeActive ();
			}
		}
		else
		{
			CheckBuy (false);
		}
	}

	void CheckBuy(bool isBuy)
	{
		buy.SetActive (!isBuy);
		if(!isBuy)
		{
			active.SetActive (false);
		}
		//buttonActive.enabled = isBuy;
	}

	void CheckActive(bool isActive)
	{
		active.SetActive (isActive);
	}

	public void Active()
	{
		if(purchased)
		{
			if (plaseActive != transform.GetSiblingIndex ()) {
				transform.parent.GetChild (plaseActive).GetComponent <PlaceShop> ().DeActive ();
			}
			plaseActive = transform.GetSiblingIndex ();
			PlayerPrefs.SetInt ("PlaceActive", plaseActive);
			if (avataCharacter != null) {
				avataCharacter.Run ();
			}
			CheckActive (true);
		}
	}

	public void DeActive()
	{
		if (avataCharacter != null) {
			avataCharacter.Idle ();
		}
		CheckActive (false);
	}

	public void Buy()
	{
		if (purchased) {
			Active ();
		} else {
			if (data.coin <= GameManager.Instan.coinTotal) {
				BuyDone ();
			} else {
				GameManager.Instan.uiManager.ShowIap ();
			}
		}
	}

	void BuyDone()
	{
		GameManager.Instan.AddCoinTotal (-data.coin);
		purchased = true;
		CheckBuy (true);
		Active ();
	}
}
