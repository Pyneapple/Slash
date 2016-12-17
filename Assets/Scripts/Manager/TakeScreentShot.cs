using UnityEngine;
using System.Collections;
using System.IO;

public class TakeScreentShot : MonoBehaviour {
	public KeyCode keyCodeTakeScreen = KeyCode.T;
	public KeyCode keyCodePause = KeyCode.Tab;
	public bool dontDestroy = false;
	string ScreenshotName = "screenshot.png";
	#if UNITY_EDITOR

	void OnEnable()
	{
		if (dontDestroy) {
			DontDestroyOnLoad (gameObject);
		}
	}

	void Update()
	{
		if(Input.GetKeyDown (keyCodeTakeScreen))
		{
			TakePhoto();
		}
		if(Input.GetKeyDown (keyCodePause))
		{
			if(Time.timeScale == 0)
			{
				Time.timeScale = 1;
			}
			else if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
			}
		}
	}

	void TakePhoto()
	{   Time.timeScale = 0;
		int number = 0;
		string pat = Application.dataPath;
		string filePath = pat.Substring (0, pat.Length - 7) + "/ScreenShot/" + Screen.width.ToString () + "x" + Screen.height.ToString ();
		if (!Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
		DirectoryInfo dir = new DirectoryInfo(filePath);
		FileInfo[] info = dir.GetFiles("*.png");
		if(info != null)
		{
			number = info.Length;
		}
		ScreenshotName = Screen.width.ToString () + "x" + Screen.height.ToString () + "_" + number.ToString () + ".png";
		string screenShotPath = filePath + "/" + ScreenshotName;
		Application.CaptureScreenshot(screenShotPath);
	}
	#endif
}
