using UnityEngine;
using System.Collections;

public class Palalax : MonoBehaviour {
	public float speedScale = 0.05f;
	GameObject player;

	Vector3 startPoint;
	GameObject GetPlayer()
	{
		if(player == null)
		{
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		return player;
	}
		
	void Awake () {
		startPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(GetPlayer () != null)
		{
			transform.position = new Vector3 (Mathf.Lerp(transform.position.x, startPoint.x - GetPlayer ().transform.position.x * speedScale, 0.2f),
				transform.position.y, transform.position.z);
		}
	}
}
