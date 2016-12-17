/*using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeGamePlay : MonoBehaviour {
	public float timeGameStart = 180;
	public Text uiTime;
	public UnityEvent TimeRezo;
	public UnityEvent addTime;
	public UnityEvent BuzzTime;
	public Animation anima;
	public int timeBuzz = 5;
	public AudioSource audioBuzz;
	public GameObject addTimeEffect;
	public Transform point;
	public Color timeColorStart;
	public Color timeColorBuzz;

	float currentTime;
	Vector2 timeMinute;

	public void StartTime()
	{
		StartCoroutine (UpdateTime ());
		currentTime = timeGameStart;
		uiTime.color = timeColorStart;
		SetTimeUi ();
	}

	public void Stop()
	{
		StopAllCoroutines ();
	}

	IEnumerator UpdateTime () {
		while(true)
		{
			yield return new WaitForSeconds (1 / GameManager.Instan.scaleTimeSmooth);
			currentTime = Mathf.Clamp (currentTime - 1, 0, timeGameStart);
			SetTimeUi ();
			if(currentTime <= timeBuzz && !audioBuzz.isPlaying)
			{
				uiTime.color = timeColorBuzz;
				audioBuzz.Play ();
				BuzzTime.Invoke ();
			}
			if(currentTime <= 0)
			{
				Rezo ();
			}
//			else if(currentTime <= 1 & Time.timeScale == 1)
//			{
//				if (GameManager.Instan.player.state == PlayerControl.StatePlayer.jump) {
//					Time.timeScale = 0.5f;
//					GameManager.Instan.player.GetComponent <InputControl>().enabled = false;
//				}
//				else
//				{
//					Time.timeScale = 1;
//				}
//			}
		}
	}

	void SetTimeUi()
	{
		timeMinute = ChangeTime.GetMinute ((int)currentTime);
		uiTime.text = ((timeMinute.x < 10)?("0" + timeMinute.x.ToString ()):(timeMinute.x.ToString ())) + " : " + 
			((timeMinute.y < 10) ? ("0" + timeMinute.y.ToString ()) : (timeMinute.y.ToString ()));
	}

	void Rezo()
	{
		StopAllCoroutines ();
		if(TimeRezo != null)
		{
			TimeRezo.Invoke ();
		}
		audioBuzz.Stop ();
	}

	public void AddTime(float time)
	{
//		if(currentTime < 1)
//		{
//			return;
//		}
		SmartPool.Spawn (addTimeEffect, point.position, point.rotation).GetComponent <AddTimeEffect>().SetText ((int)time);

		currentTime = Mathf.Clamp (currentTime + time, 0, timeGameStart);
		if(currentTime > timeBuzz && audioBuzz.isPlaying)
		{
			audioBuzz.Stop ();
		}
		if(addTime != null)
		{
			addTime.Invoke ();
		}
		anima.enabled = false;
		anima.enabled = true;
		anima.Play ();
	}
}
*/