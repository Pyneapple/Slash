using UnityEngine;
using System.Collections;

[System.Serializable]
public class ColorCombo
{
	public Color color;
	public Sprite sprite;
}

public class EffectUi : MonoBehaviour {
	public ColorCombo[] effectColor;
	public SpriteRenderer render;
	public Animator anima;
	public int timeDelayDestroy = 3;
	public int timeAudioEnd = 2;
	public SpriteRenderer textCombo;
	public AudioClip clipStart;
	public AudioClip clipEnd;
	public AudioSource audi;
	public GameObject doubleScore;
	float time;

	void Awake()
	{
		EventManager.StartListening ("GameOver",StopEffect);
		EventManager.StartListening ("Boom",StopEffect);
		EventManager.StartListening ("BackHome",StopEffect);
	}

	public void StartEffect(int effect, int timeEffect)
	{
		render.color = effectColor [effect - 1].color;
		textCombo.sprite = effectColor [effect - 1].sprite;
		render.enabled = true;
		time = timeEffect;
		anima.SetTrigger ("start");
		audi.clip = clipStart;
		audi.enabled = false;
		audi.enabled = true;
		StartCoroutine (UpdateEffect ());
	}

	IEnumerator UpdateEffect()
	{
		while(time > 0)
		{
			time -= 1;
			if(time == timeDelayDestroy)
			{
				anima.SetTrigger ("effectDestroy");
			}
			else if(time == timeAudioEnd)
			{
				audi.clip = clipEnd;
				audi.enabled = false;
				audi.enabled = true;
			}
			else if(time <= 0)
			{
				StopEffect ();
			}
			yield return new WaitForSeconds (1);
		}
	}

	public void DoubleScore()
	{
		doubleScore.gameObject.SetActive (true);
		StartEffect (1, GameManager.Instan.timeEffect);
	}

	public void Freeze()
	{
		StartEffect (2, GameManager.Instan.timeEffect);
	}

	public void Frenzy()
	{
		StartEffect (3, GameManager.Instan.timeEffect);
	}
	public void Coins()
	{
		StartEffect (4, GameManager.Instan.timeEffect);
	}


	public void StopEffect()
	{
		StopAllCoroutines ();
		render.enabled = false;
		textCombo.sprite = null;
		doubleScore.gameObject.SetActive (false);

	}
}
