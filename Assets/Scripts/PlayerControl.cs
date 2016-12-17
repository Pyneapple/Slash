using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {
	public enum StatePlayer{
		run,
		sit,
		jump
	}
	public StatePlayer state = StatePlayer.jump;
	public float speedMove;
	public float speedJump;
	public Transform pointUp;
	public Transform pointDown;
	public float deltaSpriteX = 0.2f;
	public float timeIdle = 0.5f;
	public bool isRight;
	public bool isUp; 
	public LayerMask mask;
	public float radiusAttack = 1;
	public int rayLength = 3;
	public Platform platform;
	public GameObject slashPrefab;
	public Color slashColor;
	public AudioSource audi;
	public AudioClip[] clip;

	Slash slash;
	Animator anima;
	Vector3 targetPosition;
	float pointHorizontal;
	Vector2 velocity;
	bool gameover = false;
	void Awake()
	{
		anima = GetComponent <Animator> ();
		pointHorizontal = (float)Screen.width / (float)Screen.height * Camera.main.orthographicSize - deltaSpriteX;
		EventManager.StartListening ("GameOver",GameOver);
		EventManager.StartListening ("Play",Play);
	}

	void GameOver()
	{
		gameover = true;
		if(state == StatePlayer.run)
		{
			Sit ();
		}
		if(state == StatePlayer.sit)
		{
			CancelInvoke ();
		}
		platform.StopAll ();
	}

	void Play()
	{
		if (this.enabled) {
			GameManager.Instan.player = this;
			gameover = false;
			StartJump ();
		}
	}

	void Update()
	{
		Move ();
	}

	void Move()
	{
		targetPosition = new Vector3 (transform.position.x + velocity.x * Time.deltaTime, transform.position.y + velocity.y * Time.deltaTime, 0);
		if(state == StatePlayer.jump)
		{
			if(targetPosition.y > pointUp.position.y)
			{
				targetPosition = new Vector3 (transform.position.x, pointUp.position.y, transform.position.z);
				Sit ();
			}
			else if(targetPosition.y < pointDown.position.y)
			{
				targetPosition = new Vector3 (transform.position.x, pointDown.position.y, transform.position.z);
				Sit ();
			}
			for(int i = 0; i < rayLength; i++)
			{
				CreateRay (new Vector3(transform.position.x - radiusAttack / 2 + i * (radiusAttack / (rayLength - 1)), transform.position.y, transform.position.z));
			}
		}
		else
		{ 
			if(gameover)
			{
				Sit ();
				return;
			}
			if(targetPosition.x > pointHorizontal)
			{
				targetPosition = new Vector3 (pointHorizontal, transform.position.y, transform.position.z);
				MoveLeft ();
			}
			else if(targetPosition.x < -pointHorizontal)
			{
				targetPosition = new Vector3 (-pointHorizontal, transform.position.y, transform.position.z);
				MoveRight ();
			}
		}
		transform.position = targetPosition;
	}

	void CreateRay(Vector3 posi)
	{
		RaycastHit2D[] raycastHit = Physics2D.RaycastAll (posi, isUp?(Vector2.up):(Vector2.down), Mathf.Abs (transform.position.y - targetPosition.y), mask);
		Debug.DrawLine (posi, new Vector3(posi.x, targetPosition.y, targetPosition.z));
		if (raycastHit.Length > 0)
		{
			for (int i = 0; i < raycastHit.Length; i++) {
				if (raycastHit [i].collider.gameObject.tag == "Fruit" ) {
					GameManager.Instan.AddCombo ();
					raycastHit [i].collider.gameObject.GetComponent <Fruit>().OnHit ();
					GameManager.Instan.AddScore (GameManager.Instan.xScore);
					GameManager.Instan.AddFruit (1);
				}
				else if (raycastHit [i].collider.gameObject.tag == "Boom" )
				{
					raycastHit [i].collider.gameObject.GetComponent <Fruit>().OnHit ();
				}
			}
		}
	}

	public void Sit()
	{
		anima.SetTrigger ("sit");
		state = StatePlayer.sit;
		transform.localScale = new Vector3 ((isRight?(1):(-1)) * Mathf.Abs (transform.localScale.x), Mathf.Abs (transform.localScale.y), transform.localScale.z);
		if(slash != null)
		{
			slash.StopSlash ();
			slash = null;
			GameManager.Instan.CheckCombo ();
		}
		if(gameover)
		{
			velocity = Vector2.zero;
			return;
		}
		if(isRight)
		{
			Invoke ("MoveRight", timeIdle);
		}
		else
		{
			Invoke ("MoveLeft", timeIdle);
		}
		if(velocity.y > 0)
		{
			platform.RunUp ();
		}
		else if(velocity.y < 0)
		{
			platform.RunDown ();
		}
		velocity = Vector2.zero;
	}
		
	public void Jump()
	{
		if(state == StatePlayer.run)
		{
			StartJump ();
		}
	}

	public void StartJump()
	{
		if (!audi.isPlaying && UnityEngine.Random.Range (0, 100) < 50) {
			audi.clip = clip [UnityEngine.Random.Range (0, clip.Length)];
			audi.Play ();
		}
		slash = SmartPool.Spawn (slashPrefab, transform.position, Quaternion.identity).GetComponent <Slash>();
		slash.StartSlash (transform);
		slash.SetColor (slashColor);
		if(isUp)
		{
			MoveDown ();
		}
		else
		{
			MoveUp ();
		}
	}

	public void MoveBack()
	{
		if(state == StatePlayer.run)
		{
			if(isRight)
			{
				MoveLeft ();
			}
			else
			{
				MoveRight ();
			}
		}
	}

	public void MoveUp()
	{
		state = StatePlayer.jump;
		anima.SetTrigger ("jump");
		transform.localScale = new Vector3 ((isRight?(-1):(1)) * Mathf.Abs (transform.localScale.x), -Mathf.Abs (transform.localScale.y), transform.localScale.z);
		isUp = true;
		velocity = new Vector2 (0, Mathf.Abs(speedJump));
		platform.JumpDown ();
	}

	public void MoveDown()
	{
		state = StatePlayer.jump;
		anima.SetTrigger ("jump");
		transform.localScale = new Vector3 ((isRight?(1):(-1)) * Mathf.Abs (transform.localScale.x), Mathf.Abs (transform.localScale.y), transform.localScale.z);
		isUp = false;
		velocity = new Vector2 (0, -Mathf.Abs(speedJump));
		platform.JumpUp ();
	}

	public void MoveLeft()
	{
		if(gameover)
		{
			Sit ();
			return;
		}
		state = StatePlayer.run;
		anima.SetTrigger ("run");
		transform.localScale = new Vector3 (-Mathf.Abs (transform.localScale.x), Mathf.Abs (transform.localScale.y), transform.localScale.z);
		isRight = false;
		velocity = new Vector2 (-Mathf.Abs(speedMove), 0);
	}

	public void MoveRight()
	{
		if(gameover)
		{
			Sit ();
			return;
		}
		state = StatePlayer.run;
		anima.SetTrigger ("run");
		transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x), Mathf.Abs (transform.localScale.y), transform.localScale.z);
		isRight = true;
		velocity = new Vector2 (Mathf.Abs(speedMove), 0);
	}
}
