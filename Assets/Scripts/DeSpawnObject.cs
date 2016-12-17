using UnityEngine;
using System.Collections;

public class DeSpawnObject : MonoBehaviour {

	public void DeSpawn()
	{
		SmartPool.Despawn (gameObject);
	}
}
