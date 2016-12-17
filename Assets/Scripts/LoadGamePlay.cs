using UnityEngine;
using System.Collections;

public class LoadGamePlay : MonoBehaviour {
	public GameObject[] players;
	public GameObject[] backGrounds;
	public GameObject[] BridgeUp;
	public GameObject[] BridgeDown;

	public void LoadGame(int player, int backGround)
	{
		LoadPlayer (player);
		LoadBackGround (backGround);
	}

	public void LoadPlayer(int index)
	{
		UnloadPlayer ();
		players [index].SetActive (true);
	}

	public void LoadBackGround(int index)
	{
		UnLoadMap ();
		backGrounds [index].SetActive (true);
		BridgeUp [index].SetActive (true);
		BridgeDown [index].SetActive (true);
	}

	public void UnloadGame()
	{
		UnloadPlayer ();
		UnLoadMap ();
	}

	public void UnloadPlayer()
	{
		foreach(GameObject player in players)
		{
			if(player.activeSelf)
			{
				player.SetActive (false);
			}
		}
	}

	public void UnLoadMap()
	{
		for(int i = 0; i < backGrounds.Length; i++)
		{
			if(backGrounds[i].activeSelf)
			{
				backGrounds[i].SetActive (false);
			}
			if(BridgeUp[i].activeSelf)
			{
				BridgeUp [i].SetActive (false);
			}
			if(BridgeDown[i].activeSelf)
			{
				BridgeDown [i].SetActive (false);
			}
		}
	}
}
