using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour {
	public GameObject character;
	public GameObject place;
	public GameObject iap;

	void Awake()
	{
		ResetShop ();
		Character ();
	}

	public void ResetShop()
	{
		if (character != null) {
			character.SetActive (false);
		}
		if (place != null) {
			place.SetActive (false);
		}
		if (iap != null) {
			iap.SetActive (false);
		}
	}

	public void Character()
	{
		if(character.activeSelf)
		{
			return;
		}
		ResetShop ();
		character.SetActive (true);
	}

	public void Places()
	{
		if(place.activeSelf)
		{
			return;
		}
		ResetShop ();
		place.SetActive (true);
	}

	public void Iap()
	{
		if(iap.activeSelf)
		{
			return;
		}
		ResetShop ();
		iap.SetActive (true);
	}
}
