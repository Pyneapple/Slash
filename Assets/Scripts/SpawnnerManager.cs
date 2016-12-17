using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FruitRandom
{
	public int lengt;
	[Range(0,100)]
	public float random;
}

public class SpawnnerManager : MonoBehaviour {
	public enum TypeItem
	{
		Fruit,
		Boom,
		Item
	}
	public TypeItem item = TypeItem.Fruit;
	public Transform listSpawnPoint;
	public Fruit[] listFruit;
	public FruitRandom[] fruitRandom; 
	public Vector2 speedSpawn;
	public Vector2 velocityFruit;
	public float gravityFruit;
	public AudioSource audi;

	void Awake()
	{
		if (item != TypeItem.Fruit) {
			EventManager.StartListening ("Effect",StopSpawmEffectItem);
		}
		EventManager.StartListening ("GameOver",StopSpawm);
		EventManager.StartListening ("BackHome",StopSpawm);
		EventManager.StartListening ("Play",StartSpawn);
	}

	void StopSpawm()
	{
		CancelInvoke ("SpawnRandom");
	}

	void StopSpawmEffectItem()
	{
		StopSpawm ();
		EventManager.StopListening ("Effect",StopSpawm);
		EventManager.StartListening ("NoneEffect",StartSpawn);
	}

	void StartSpawn () {
		StopSpawm ();
		if (item != TypeItem.Fruit) {
			EventManager.StopListening ("NoneEffect", StartSpawn);
			Invoke ("SpawnRandom", Random.Range (speedSpawn.x, speedSpawn.y) * GameManager.Instan.speedSpawnBonus / GameManager.Instan.timeScaleLevel / GameManager.Instan.timeScaleLevel);
		} else {
			Invoke ("SpawnRandom", 2);
		}
	}

	void SpawnRandom()
	{
		if(!GameManager.Instan.isPlaying)
		{
			return;
		}
		audi.Play ();
		float ran = Random.Range (0, 100);
		for(int i = 0; i < fruitRandom.Length; i++)
		{
			if(ran <= fruitRandom[i].random)
			{
				//float spe = Random.Range (velocityFruit.x, velocityFruit.y);
				for(int j = 0; j < fruitRandom[i].lengt; j++)
				{
					Spawn(Random.Range (velocityFruit.x, velocityFruit.y));
				}
				Invoke("SpawnRandom",Random.Range (speedSpawn.x, speedSpawn.y) * GameManager.Instan.speedSpawnBonus / GameManager.Instan.timeScaleLevel / GameManager.Instan.timeScaleLevel);
				return;
			}
		}
	}

	void Spawn(float speed)
	{
		Transform pointSpawn = listSpawnPoint.GetChild (Random.Range (0, listSpawnPoint.childCount));
		SmartPool.Spawn (listFruit[Random.Range (0, listFruit.Length)].gameObject, pointSpawn.position, pointSpawn.rotation)
			.GetComponent <Fruit>().StartFruit (new Vector2(speed * Mathf.Cos (pointSpawn.eulerAngles.z * Mathf.Deg2Rad),
				speed * Mathf.Sin (pointSpawn.eulerAngles.z * Mathf.Deg2Rad)), gravityFruit);
	}
}
