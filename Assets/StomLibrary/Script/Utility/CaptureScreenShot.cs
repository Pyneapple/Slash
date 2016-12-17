#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Stom;

/// <summary>
/// Notice: This cript only work if gameview have defaul settup is free aspect
/// To capturescreen shot: Hit S on Keyboard
/// Please wait when capture finish
/// </summary>

public class CaptureScreenshot : MonoBehaviour {

    public Vector2[] screenResolutions;
    public string folderPath;

    //The size of the toolbar above the game view, excluding the OS border.
    private int tabHeight = 22;

    //Desired window position
    private readonly Vector2 gamePosition = new Vector2(0, 50);
    // Time to wait screen resize
    private const float timeWait = 0.5f;

    private float progress = 0.0f;
    private bool isCapturing;

    //Get the size of the window borders. Changes depending on the OS.
#if UNITY_STANDALONE_WIN
	//Windows settings
	private int osBorderWidth = 5;
#elif UNITY_STANDALONE_OSX
	//Mac settings (untested)
	private int osBorderWidth = 0; //OSX windows are borderless.
#else
    //Linux / other platform; sizes change depending on the variant you're running
    private int osBorderWidth = 5;
#endif

    void Start()
    {
        // Initial size game view
        EditorWindow gameView = GetMainGameView();
        //When minSize and maxSize are the same, no OS border is applied to the window.
        gameView.minSize = new Vector2(1080, 720 + tabHeight - osBorderWidth);
        gameView.maxSize = gameView.minSize;
        // Apply resize
        Rect newPos = new Rect(gamePosition.x, gamePosition.y - tabHeight, 1080, 720 + tabHeight - osBorderWidth);
        gameView.position = newPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isCapturing)
            {
                isCapturing = true;
                TimeScaleManager.StopPlaying();
                // Start capture
                StartCoroutine(CaptureScreen());
            }
        }
    }

    /// <summary>
    ///  Method start capture
    /// </summary>
    /// <returns></returns>
    private IEnumerator CaptureScreen()
    {
        StartCoroutine(DisplayProcessbar()); 
        // Start resize
        for (int i = 0; i < screenResolutions.Length; i++)
        {
            yield return StartCoroutine(ResizeScreen(screenResolutions[i]));
            // Wait unscale time
            float timeStartWait = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - timeStartWait < timeWait)
                yield return null;
        }
        // Restore
        EditorWindow gameView = GetMainGameView();
        //When minSize and maxSize are the same, no OS border is applied to the window.
        gameView.minSize = new Vector2(1080, 720 + tabHeight - osBorderWidth);
        gameView.maxSize = gameView.minSize;
        // Apply resize
        Rect newPos = new Rect(gamePosition.x, gamePosition.y - tabHeight, 1080, 720 + tabHeight - osBorderWidth);
        gameView.position = newPos;
    }

    private IEnumerator DisplayProcessbar()
    {
        float _timeProcess = (screenResolutions.Length) * timeWait*2;
        float _timeStart = (float) EditorApplication.timeSinceStartup;
        progress = 0.0f;

        while (progress < _timeProcess)
        {           
            progress = (float) EditorApplication.timeSinceStartup - _timeStart;
            EditorUtility.DisplayProgressBar("Capture processing ...", "Shows a progress bar for the given seconds",
                                                  (float)(progress / _timeProcess));
            yield return null;
        }

        EditorUtility.DisplayDialog("Capture ScreenShot", "Completed!", "Ok");
        EditorUtility.ClearProgressBar();

        // Resume play
        TimeScaleManager.ResumePlaying();
    }

    /// <summary>
    /// Method resize gameview base on resolution that you chose
    /// </summary>
    /// <param name="resolution"></param>
    /// <returns></returns>
    private IEnumerator ResizeScreen(Vector2 resolution)
    {
        EditorWindow gameView = GetMainGameView();
        gameView.titleContent = new GUIContent("CaptureScreen");

        //When minSize and maxSize are the same, no OS border is applied to the window.
        gameView.minSize = new Vector2(resolution.x, resolution.y + tabHeight - osBorderWidth);
        gameView.maxSize = gameView.minSize;

        Rect newPos = new Rect(gamePosition.x, gamePosition.y - tabHeight, resolution.x, resolution.y + tabHeight - osBorderWidth);
        gameView.position = newPos;

        yield return StartCoroutine(TakeScreenShot());

        // Reset, now can capture again
        isCapturing = false;
    }

    /// <summary>
    /// Method capture screen to save in folder
    /// </summary>
    /// <returns></returns>
    private IEnumerator TakeScreenShot()
    {
        // Wait unscale time
        float timeStartWait = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - timeStartWait < timeWait)
            yield return null;

        string _screenshotName = "screenshot.png";
        int _number = 0;

        string filePath = folderPath + Screen.width.ToString() + "x" + Screen.height.ToString();
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] info = dir.GetFiles("*.png");
        if (info != null)
            _number = info.Length;

        if(_number > 9)
            _screenshotName = Screen.width.ToString() + "x" + Screen.height.ToString() + "_" + (_number+1).ToString() + ".png";
        else
            _screenshotName = Screen.width.ToString() + "x" + Screen.height.ToString() + "_0" + (_number+1).ToString() + ".png";

        string screenShotPath = filePath + "/" + _screenshotName;
        Application.CaptureScreenshot(screenShotPath);      
    }

    /// <summary>
    /// Method get Gameview component in editor
    /// </summary>
    /// <returns></returns>
    public static EditorWindow GetMainGameView()
    {
        //Creates a game window. Only works if there isn't one already.
        EditorApplication.ExecuteMenuItem("Window/Game");

        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null, null);
        return (EditorWindow)Res;
    }
}
#endif
