/*
Clicked 'Unity Demo' which fucked up some objects :( )
disabled IAP calls in CharacterShop.cs
Disabled IAP service in Unity
*/
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class ComboManager
{
	public int hit;
	public GameObject effect;
	public UnityEvent action;

	public void ActionCombo(Vector3 pointEffect)
	{
		if (effect != null) {
			SmartPool.Spawn (effect, pointEffect, Quaternion.identity);
		}
		if(action != null)
		{
			action.Invoke ();
		}
	}
}

[System.Serializable]
public class ComboXCoin
{
	public int hit;
	public int xCombo;
	public Sprite effect;
	public UnityEvent action;

	public void ActionCombo()
	{
		if(action != null)
		{
			action.Invoke ();
		}
	}
}

public class GameManager : MonoBehaviour {
	public int Lives = 3;
	public float speedTimeLevel = 0.01f;
	public float timeScaleLevel = 1;
	public int timeEffect = 10;
	public int timeBoom = 10;
	public SettingGamePlay defaultSetting;
	public SettingGamePlay effectSetting;
	public float speedSpawnBonus = 1;
	public float scaleTimeSmooth = 1;
	public float scaleSpeedFruit = 1;
	public int xScore = 1;
	public int combo1 = 0;                // single slash
	public int combo2 = 0;                // multiple slash
	public int fruit = 0;
	public int combo = 0;
	public int xCoin = 0;
	public ComboManager[] comboManager1;
	public ComboXCoin[] comboManager2;
	public EffectUi effectUi;
	public UiManager uiManager;
	public LoadGamePlay loadGamePlay;
	public int coinTotal;
	public int coinLevel;
	public PlayerControl player;
	public XcomboEffect xCoinEffect;
	public bool isPlaying = false;
	public int Score

	{
		get
		{
			return score;
		}
	}
	static GameManager instan;
	public static GameManager Instan
	{
		get
		{
			if(instan == null)
			{
				instan = FindObjectOfType <GameManager> ();
			}
			return instan;
		}
	}

	int score = 0;

	[ContextMenu("Delete All")]
	public void DeleteAllkey()
	{
		PlayerPrefs.DeleteAll ();
	}

	void Awake()
	{
		if(PlayerPrefs.HasKey ("CoinTotal"))
		{
			coinTotal = PlayerPrefs.GetInt ("CoinTotal");
		}
		AddCoinTotal (0);
		Application.targetFrameRate = 60;
	}

	void Update()
	{
		if(isPlaying)
		{
			timeScaleLevel = Mathf.Clamp(timeScaleLevel + speedTimeLevel * Time.deltaTime * scaleTimeSmooth, defaultSetting.startTimeScaleLevel, effectSetting.startTimeScaleLevel);
		}
	}

	public void PlayGame()
	{
		SmartPool.ReturnPoolAll ();
		score = 0;
		fruit = 0;
		combo = 0;
		combo1 = 0;
		combo2 = 0;
		xCoin = 0;
		coinLevel = 0;
		Lives = 3;
		timeScaleLevel = defaultSetting.startTimeScaleLevel;
		scaleTimeSmooth = defaultSetting.scaleTimeSmooth;
		speedSpawnBonus = defaultSetting.speedSpawnBonus;
		scaleSpeedFruit = defaultSetting.scaleSpeedFruit;
		xScore = defaultSetting.xScore;
		isPlaying = true;
		loadGamePlay.gameObject.SetActive (true);
		loadGamePlay.LoadGame (CharacterShop.characterActive, PlaceShop.plaseActive);
	}

	public void ResetEffect()
	{
		EventManager.TriggerEvent ("NoneEffect");
		scaleTimeSmooth = defaultSetting.scaleTimeSmooth;
		speedSpawnBonus = defaultSetting.speedSpawnBonus;
		scaleSpeedFruit = defaultSetting.scaleSpeedFruit;
		xScore = defaultSetting.xScore;
	}

	public void AddScore(int sco)
	{
		score += sco * xCoin;
		uiManager.UpdateScore ();


	}
		
	public void Freeze()
	{
		effectUi.Freeze();
		EventManager.TriggerEvent ("Effect");
		scaleTimeSmooth = effectSetting.scaleTimeSmooth;
		speedSpawnBonus = defaultSetting.speedSpawnBonus / 2;
		Invoke ("ResetEffect", timeEffect);
	}
		
	public void Frenzy()
	{
		effectUi.Frenzy ();
		EventManager.TriggerEvent ("Effect");
		speedSpawnBonus = effectSetting.speedSpawnBonus;
		scaleSpeedFruit = effectSetting.scaleSpeedFruit;
		Invoke ("ResetEffect", timeEffect);
	}

	public void DoubleScore()
	{
		effectUi.DoubleScore ();
		EventManager.TriggerEvent ("Effect");
		xScore = effectSetting.xScore;
		speedSpawnBonus = defaultSetting.speedSpawnBonus / 1.5f;
		Invoke ("ResetEffect", timeEffect);
	}

	public void Coins()
	{
		//effectUi.Frenzy (); 
		//EventManager.TriggerEvent ("Effect");
		coinLevel = coinLevel + 30;
		uiManager.UpdateCoinPlay ();
		//scaleSpeedFruit = effectSetting.scaleSpeedFruit;
		//Invoke ("ResetEffect", timeEffect);

	}

	public void addToCoinsTotal(int coi)
	{
			coinLevel = coinLevel + coi;
		uiManager.UpdateCoinPlay ();
	}


	public void DeXscoreBonus()
	{
		EventManager.TriggerEvent ("NoneEffect");
		xScore = defaultSetting.xScore;
		scaleSpeedFruit = defaultSetting.scaleSpeedFruit;
	}

	public void ResetCombo()
	{
		combo1 = 0;
		combo2 = 0;
		xCoin = 1;
		xCoinEffect.DePlay ();
	}

	public void CheckCombo()
	{
		if (combo1 <= 0) {
			ResetCombo ();
		} else {
			combo2++;
			for (int i = comboManager1.Length - 1; i >= 0; i--) {
				if (combo1 >= comboManager1 [i].hit) {
					comboManager1 [i].ActionCombo (new Vector3(player.transform.position.x, 0, 0));
					AddCombo (1);
					break;
				}
			}
			for (int j = comboManager2.Length - 1; j >= 0; j--) {
				if (combo2 >= comboManager2 [j].hit) {
					comboManager2 [j].ActionCombo ();
					xCoin = comboManager2 [j].xCombo;
					xCoinEffect.Play (comboManager2 [j].effect);
					//GameManager.Instan.AddCoinLevel (combo1);
					break;
				}
			}
		}
		combo1 = 0;
	}

	public void AddCombo()
	{
		combo1++;
	}

	public void ResetBoom()
	{
		ResetCombo ();
		ResetEffect ();
//		AddTime (-timeBoom);
		RemoveLife ();
		if (timeScaleLevel <= 0.8f){

			} else{
		timeScaleLevel = 0.8f;
	}
	
	}
	public void RemoveLife(){
	Lives = Lives - 1;
		uiManager.UpdateLives ();
		if (Lives <=0){

			uiManager.GameOver ();
		}

	}

	public void GameOver()
	{
		EventManager.TriggerEvent ("GameOver");
		uiManager.ShowGameOver ();
		AddCoinTotal (coinLevel);
		isPlaying = false;

	}

	public void AddCoinTotal(int coi)
	{
		coinTotal += coi;
		uiManager.UpdateCoinShop ();
		PlayerPrefs.SetInt ("CoinTotal", coinTotal);
	}

	public void AddCoinLevel(int coi)
	{
		score += coi * combo2;
		uiManager.UpdateCoinPlay ();
	}

	public void AddFruit(int fru)
	{
		fruit += fru;
	}

	public void AddCombo(int comb)
	{
		combo += comb;

	}

	//RESET PLAYER PREFS
	public void resetMyPlayer()
	{
		PlayerPrefs.DeleteAll();
	}



//Pynetree additions



/*	public void AddTime(int tim)
	{
		uiManager.timeGamePlay.AddTime (tim);
	}*/
}
