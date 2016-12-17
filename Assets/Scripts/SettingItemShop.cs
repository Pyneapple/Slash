using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingItemShop : MonoBehaviour {
	public Color color1;
	public Color color2;

	void Awake()
	{
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Image>().color = (i % 2 == 0) ? (color1) : (color2);
		}
	}
}
