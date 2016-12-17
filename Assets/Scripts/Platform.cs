using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	public Animator upPlatform;
	public Animator downPlatform;

	public void JumpUp()
	{
		upPlatform.SetBool ("run", false);
	}

	public void JumpDown()
	{
		downPlatform.SetBool ("run", false);
	}

	public void RunUp()
	{
		upPlatform.SetBool ("run", true);
	}

	public void RunDown()
	{
		downPlatform.SetBool ("run", true);
	}

	public void StopAll()
	{
		downPlatform.SetBool ("run", false);
		upPlatform.SetBool ("run", false);
	}
}
