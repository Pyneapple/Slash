using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvataCharacter : MonoBehaviour {
	public Animator anima;

	[ContextMenu("Idle")]
	public void Idle()
	{
		anima.SetTrigger ("idle");
	}

	[ContextMenu("Run")]
	public void Run()
	{
		anima.SetTrigger ("run");
	}
}
