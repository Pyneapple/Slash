using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Fruit : MonoBehaviour {
	public FruitCrack fruitCrack;
	public Vector2 randomAngle;
	public UnityEvent hit;
	Vector2 velocity;
	float gravity;
	float speedAngle;

	CameraControl cam;
	Animator bom;
	CircleCollider2D colliderBom;

	void Awake()
	{
		if (gameObject.tag == "Boom") {
			cam = Camera.main.GetComponent <CameraControl> ();
			bom = GetComponent <Animator> ();
			colliderBom = GetComponent <CircleCollider2D> ();
		}
	}

	public void StartFruit(Vector2 veloc, float gra)
	{
		if(gameObject.tag == "Boom")
		{
			colliderBom.enabled = true;	
		}
		velocity = veloc;
		gravity = gra;
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, Random.Range (-45, 45));
		speedAngle = Random.Range (randomAngle.x, randomAngle.y);
	}

	void Update()
	{
		transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime * GameManager.Instan.scaleTimeSmooth * GameManager.Instan.scaleSpeedFruit * GameManager.Instan.timeScaleLevel,
			transform.position.y + velocity.y * Time.deltaTime * GameManager.Instan.scaleTimeSmooth * GameManager.Instan.scaleSpeedFruit * GameManager.Instan.timeScaleLevel, transform.position.z);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y,
			transform.eulerAngles.z + speedAngle * Time.deltaTime * GameManager.Instan.scaleTimeSmooth * GameManager.Instan.scaleSpeedFruit * GameManager.Instan.timeScaleLevel);
		velocity = new Vector2 (velocity.x, velocity.y + gravity * Time.deltaTime * GameManager.Instan.scaleTimeSmooth * GameManager.Instan.scaleSpeedFruit * GameManager.Instan.timeScaleLevel);
		if(transform.position.y < -Camera.main.orthographicSize * 1.5f)
		{
			DeSpawn ();
		}
	} 

	void DeSpawn()
	{
		SmartPool.Despawn (gameObject);
	}

	public void OnHit()
	{
		if (fruitCrack != null) {
			SmartPool.Spawn (fruitCrack.gameObject, transform.position, transform.rotation);
		}
		if(hit != null)
		{
			hit.Invoke ();
		}
		if(gameObject.tag == "Fruit")
		{
			DeSpawn ();
		}
		else if(gameObject.tag == "Boom")
		{
			StartBoom ();
		}
	}

	void StartBoom()
	{
		EventManager.TriggerEvent ("Boom");
		colliderBom.enabled = false;
		GameManager.Instan.ResetBoom ();
		cam.StartBoom ();
		bom.enabled = true;
		Time.timeScale = 0;
	}

	public void StopBoom()
	{
		bom.enabled = false;
		cam.StopBoom ();
		Time.timeScale = 1;
		DeSpawn ();
	}

	public void Smooth()
	{
		GameManager.Instan.Freeze ();
	}

	public void SpawnBonus()
	{
		GameManager.Instan.Frenzy ();
	}

	public void DoubleScore()
	{
		GameManager.Instan.DoubleScore ();
	}

	public void CoinFrenzy()
	{
		GameManager.Instan.Coins ();
	}
		
}
