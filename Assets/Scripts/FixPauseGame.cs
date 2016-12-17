using UnityEngine;
using System.Collections;

public class FixPauseGame : MonoBehaviour {
	public Animator anima;
	void OnEnable()
	{
		EventManager.StartListening ("Pause", Pause);
	}

	void OnDisable()
	{
		EventManager.StopListening ("Pause", Pause);
		EventManager.StopListening ("UnPause", UnPause);
	}

	void Pause()
	{
		anima.speed = 0;
		EventManager.StartListening ("UnPause", UnPause);
	}

	void UnPause()
	{
		anima.speed = 1;
	}
}
