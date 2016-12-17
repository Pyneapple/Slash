using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {
	public Image imaButton;
	public Sprite sound;
	public Sprite unSound;


	void OnEnable()
	{
		AudioListener.pause = PlayerPrefs.HasKey ("sound");
		CheckSound ();
	}

	void CheckSound()
	{
		if (AudioListener.pause) {
			imaButton.sprite = unSound;
			if(!PlayerPrefs.HasKey ("sound"))
			{
				PlayerPrefs.SetInt ("sound", 1);
			}
		} else {
			imaButton.sprite = sound;
			if(PlayerPrefs.HasKey ("sound"))
			{
				PlayerPrefs.DeleteKey ("sound");
			}
		}
	}

	public void ClickSound()
	{
		AudioListener.pause = !AudioListener.pause;
		CheckSound ();
	}
}
