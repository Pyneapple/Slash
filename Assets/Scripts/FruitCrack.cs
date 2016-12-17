using UnityEngine;
using System.Collections;

[System.Serializable]
public class EffectComboFruit
{
	public int hit;
	public GameObject effect;
}

public class FruitCrack : MonoBehaviour {
	public float timeLife = 2;
	public Rigidbody2D fruit1;
	public Rigidbody2D fruit2;
	public GameObject particleFruit;
	public EffectComboFruit[] effectCombos;
	public Color colorFruit;
	public Vector2 velocityX;
	public Vector2 velocityY;
	public Vector2 velocityAngle;

	Vector3 posi1;
	Vector3 rota1;
	Vector3 posi2;
	Vector3 rota2;
	GameObject particle;

	void Awake()
	{
		posi1 = fruit1.transform.localPosition;
		posi2 = fruit2.transform.localPosition;
		rota1 = fruit1.transform.localEulerAngles;
		rota2 = fruit2.transform.localEulerAngles;
	}

	// Use this for initialization
	void OnEnable () {
		fruit1.transform.localPosition = posi1;
		fruit2.transform.localPosition = posi2;
		fruit1.transform.localEulerAngles = rota1;
		fruit2.transform.localEulerAngles = rota2;
		fruit1.gameObject.SetActive (true);
		fruit2.gameObject.SetActive (true);
		if (fruit1.transform.position.x >= fruit2.transform.position.x) 
		{
			AddForce (fruit1, true);
			AddForce (fruit2, false);
		}
		else
		{
			AddForce (fruit1, false);
			AddForce (fruit2, true);
		}
		particle = SmartPool.Spawn (particleFruit.gameObject.gameObject, transform.position, Quaternion.identity);
		for(int i = effectCombos.Length - 1; i >= 0; i--)
		{
			if(GameManager.Instan.combo1 >= effectCombos[i].hit)
			{
				if(effectCombos[i].effect != null)
				{
					SmartPool.Spawn (effectCombos[i].effect, transform.position, transform.rotation).GetComponent <ComboFruit>().StartEffect (colorFruit);
				}
				break;
			}
		}
		particle.GetComponent <ParticleCrush>().Play (colorFruit);
		Invoke ("Despawn", timeLife);
	}

	void OnDisable()
	{
		fruit1.gameObject.SetActive (false);
		fruit2.gameObject.SetActive (false);
	}

	void Despawn()
	{
		SmartPool.Despawn (particle);
		SmartPool.Despawn (gameObject);
	}

	void AddForce(Rigidbody2D rigid, bool isRight)
	{
		rigid.angularVelocity = (isRight?(-1):(1)) * Random.Range (velocityAngle.x, velocityAngle.y);
		rigid.velocity = new Vector2 ((isRight?(1):(-1)) * Random.Range (velocityX.x, velocityX.y), Random.Range (velocityY.x, velocityY.y));
	}
}
