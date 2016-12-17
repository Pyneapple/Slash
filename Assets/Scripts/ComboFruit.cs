using UnityEngine;
using System.Collections;

public class ComboFruit : MonoBehaviour {
	public float timeLife = 1.5f;
	public SpriteRenderer render;

	public void StartEffect(Color colo)
	{
		render.color = colo;
		Invoke ("Despawn", timeLife);
	}

	void Despawn()
	{
		SmartPool.Despawn (gameObject);
	}
}
