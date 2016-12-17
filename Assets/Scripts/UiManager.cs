using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
	public Text scoreGamePlay;
	public Text coinGamePlay;
	public Text coinShop;
	public Text scoreGameOver;
	public Text bestScore;
	public Text fruit;
	public Text combo;
	public Text coinGameOver;
//	public TimeGamePlay timeGamePlay;
	public ShopManager shop;
	public GameObject gamePlayUi;
	public GameObject homeUi;
	public GameObject gameOverUi;
	public AudioSource audioStart;
	public Animator anima;
	bool currentPause = false;
	public Text scoreLives;
	public Text high_score;

	void Awake () {
//		timeGamePlay.TimeRezo.AddListener (GameOver);
		anima.SetBool ("home", true);
	}

	public void GameOver()
	{
		anima.SetBool ("play", false);
		anima.SetBool ("gameOver", true);
		GameManager.Instan.GameOver ();
//		AdsTv.instance.ShowAd ();
	}
	
	public void StartGamePlay()
	{
		anima.SetBool ("home", false);
		anima.SetBool ("play", true);
		Time.timeScale = 1;
		gamePlayUi.SetActive (true);
		homeUi.SetActive (false);
		GameManager.Instan.PlayGame ();
		UpdateScore ();
		UpdateLives ();
		UpdateCoinPlay ();
		EventManager.TriggerEvent ("Play");
		audioStart.Stop ();
		audioStart.Play ();
//		timeGamePlay.StartTime ();
		UnloadResource ();
//		AdsTv.instance.LoadAd ();
		high_score.text = (PlayerPrefs.HasKey ("BestScore")) ? (PlayerPrefs.GetInt ("BestScore").ToString ()) : (GameManager.Instan.Score.ToString ());
	}



	public void ShowIap()
	{
		shop.Iap ();
	}

			public void UpdateScore()
	{
		scoreGamePlay.text = GameManager.Instan.Score.ToString ();

	}

				public void UpdateLives()
	{
		scoreLives.text = GameManager.Instan.Lives.ToString ();

	}

	public void UpdateCoinShop()
	{
		coinShop.text = GameManager.Instan.coinTotal.ToString ();
	}


	public void UpdateCoinPlay()
	{
		coinGamePlay.text = GameManager.Instan.coinLevel.ToString ();
	}

	public void ShowGameOver()
	{
		gameOverUi.SetActive (true);
		scoreGameOver.text = GameManager.Instan.Score.ToString ();
		bestScore.text = (PlayerPrefs.HasKey ("BestScore")) ? (PlayerPrefs.GetInt ("BestScore").ToString ()) : (GameManager.Instan.Score.ToString ());
		if(!PlayerPrefs.HasKey ("BestScore") || GameManager.Instan.Score > PlayerPrefs.GetInt ("BestScore"))
		{
			PlayerPrefs.SetInt ("BestScore", GameManager.Instan.Score);
		}
		fruit.text = GameManager.Instan.fruit.ToString ();
		combo.text = GameManager.Instan.combo.ToString ();
		coinGameOver.text = GameManager.Instan.coinLevel.ToString ();
		Stom.NativePlugin.GameService.SetLeaderboardValue (Stom.NativePlugin.LeaderboardType.HIGHT_SCORE, GameManager.Instan.Score);
		Stom.NativePlugin.GameService.SetLeaderboardValue (Stom.NativePlugin.LeaderboardType.FRUIT_SLASHED, GameManager.Instan.fruit);
		Invoke ("EndGameOver", 0.5f);
	}

	void EndGameOver()
	{
		Stom.NativePlugin.EventManager.TriggerEventService (Stom.NativePlugin.ServiceEvent.GAMEOVER);
	}


	public void BackHome()
	{
		SmartPool.ReturnPoolAll ();
		Time.timeScale = 1;
		EventManager.TriggerEvent ("BackHome");
		Invoke ("DelayBackHome", 1);
//		timeGamePlay.Stop ();
		gameOverUi.SetActive (false);
		gamePlayUi.SetActive (false);
		homeUi.SetActive (true);
		anima.SetBool ("home", true);
		anima.SetBool ("play", false);
		anima.SetBool ("pause", false);
		anima.SetBool ("gameOver", false);
		UnloadResource();
	}

	void DelayBackHome()
	{
		GameManager.Instan.loadGamePlay.gameObject.SetActive (false);
	}

	public void RePlay()
	{
		StartGamePlay ();
		anima.SetBool ("play", true);
		anima.SetBool ("gameOver", false);
	}

	public void Pause()
	{
		currentPause = (Time.timeScale == 0);
		Time.timeScale = 0;
		anima.SetBool ("pause", true);
		EventManager.TriggerEvent ("Pause");
	}

	public void UnPauseGame()
	{
		if (!currentPause) {
			Time.timeScale = 1;
		}
		anima.SetBool ("pause", false);
		EventManager.TriggerEvent ("UnPause");
	}

	void UnloadResource()
	{
		Resources.UnloadUnusedAssets ();
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
