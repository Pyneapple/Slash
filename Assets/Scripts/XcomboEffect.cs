using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class XcomboEffect : MonoBehaviour {
	public Image ren;
	public Animation anima;
	public bool isCombo = false;
	public UnityEvent ComboAction;
	public UnityEvent NoneComboAction;

	void Awake()
	{
		EventManager.StartListening ("Play",DePlay);
	}

	public void Play(Sprite sprite)
	{
		if(ComboAction != null)
		{
			ComboAction.Invoke ();
		}
		isCombo = true;
		ren.enabled = true;
		ren.sprite = sprite;
		anima.Stop ();
		anima.Play ();
	}

	public void DePlay()
	{
		if(!isCombo)
		{
			return;
		}
		if(NoneComboAction != null)
		{
			NoneComboAction.Invoke ();
		}
		ren.enabled = false;
		isCombo = false;
	}
}
