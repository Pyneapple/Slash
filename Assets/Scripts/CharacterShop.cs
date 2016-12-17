using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterShop : MonoBehaviour {
	public static int characterActive;
	public DataShop dataCharacter;
	public AvataCharacter avataCharacter;
	public Text nameCharacter;
	public Text description;
	public Text coinText;
	public GameObject buy;
	public GameObject active;
	public Button buttonActive;
	public GameObject popupSave;
	public bool purchased
	{
		get{
			return PlayerPrefs.HasKey ("character" + transform.GetSiblingIndex ().ToString ());
		}
		set
		{
			if(value)
			{
				PlayerPrefs.SetInt ("character" + transform.GetSiblingIndex ().ToString (), 1);
			}
		}
	}

	void Awake()
	{
		if(transform.GetSiblingIndex () == 0)
		{
			if(PlayerPrefs.HasKey ("CharacterActive"))
			{
				characterActive = PlayerPrefs.GetInt ("CharacterActive");
			}
			else
			{
				characterActive = 0;
			}
		}
		if(dataCharacter.purchased)
		{
			purchased = true;
		}
	}

/*	void Restore()
	{
		if(Stom.NativePlugin.IAPNoneConsumeProcess.CheckNoneConsume (transform.GetSiblingIndex ()))
		{
			purchased = true;
		}
	}*/

/*	void OnEnable()
	{
		if(!purchased)
		{
			Restore ();
		}
		SetupCharacter ();
	}*/

	void Start()
	{
		SetupCharacter ();
	}

	public void SetupCharacter()
	{
		nameCharacter.text = dataCharacter.nameProduc;
		description.text = dataCharacter.description;
		coinText.text = dataCharacter.coin.ToString ();

		if(purchased)
		{
			CheckBuy (true);
			if(characterActive == transform.GetSiblingIndex ())
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
			if (characterActive != transform.GetSiblingIndex ()) {
				transform.parent.GetChild (characterActive).GetComponent <CharacterShop> ().DeActive ();
			}
			characterActive = transform.GetSiblingIndex ();
			PlayerPrefs.SetInt ("CharacterActive", characterActive);
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

	public void BuyCharacter()
	{
		if (purchased) {
			Active ();
		} else {
			if (dataCharacter.coin <= GameManager.Instan.coinTotal) {
				BuyDone ();
			} else {
				GameManager.Instan.uiManager.ShowIap ();
			}
		}
	}

	void BuyDone()
	{
		GameManager.Instan.AddCoinTotal (-dataCharacter.coin);
		purchased = true;
		CheckBuy (true);
		Active ();
		if(popupSave != null)
		{
			popupSave.SetActive (true);
		}
	}
}
