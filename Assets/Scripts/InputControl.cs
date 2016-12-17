using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
#if UNITY_TVOS
using EasyInput.Core;
#endif

public class InputControl : MonoBehaviour {
	public PlayerControl playerMovement;
	public const float deltaTouch = 0.1f;

	void Awake()
	{
		EventManager.StartListening ("GameOver",DeControl);
		EventManager.StartListening ("Play",Active);
		EventManager.StartListening ("Pause",DeControl);
		EventManager.StartListening ("UnPause",Active);
	}

	void Active()
	{
		this.enabled = true;
	}

	void DeControl()
	{
		this.enabled = false;
	}

	#if UNITY_EDITOR
	void Update () {
		if(EventSystem.current.currentSelectedGameObject != null)
		{
			return;
		}
		if(Input.GetKeyDown (KeyCode.Space))
		{
			playerMovement.MoveBack ();
		}
		else if(Input.GetMouseButtonDown (0))
		{
			playerMovement.Jump ();
		}

	}
	#else
	void Update () {
	if(EventSystem.current.currentSelectedGameObject != null)
	{
	return;
	}
	for(int i = 0; i < Input.touchCount; i++)
	{
	if(Input.GetTouch (i).phase == TouchPhase.Began)
	{
	if(Input.GetTouch (i).position.x < Screen.width / 2)
	{
	playerMovement.MoveBack ();
	}
	else
	{
	playerMovement.Jump ();
	}
	}
	}
	}
	#endif
}
