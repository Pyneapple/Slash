using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public Animator anima;

	public void StartBoom()
	{
		anima.SetBool ("boom", true);
	}

	public void StopBoom()
	{
		anima.SetBool ("boom", false);
	}
}
