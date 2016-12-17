#if UNITY_EDITOR
#define EDITOR_ONLY
#endif


using UnityEngine;
using System.Diagnostics;
using UnityEngine.Internal;
using System;

#if REPLACE_LOG
public static class Debug
{
#region Debug Console Unity
    //
    // Summary:
    //     Opens or closes developer console.
    public static bool developerConsoleVisible { get { return UnityEngine.Debug.developerConsoleVisible; } set { UnityEngine.Debug.developerConsoleVisible = value; } }
    //
    // Summary:
    //     In the Build Settings dialog there is a check box called "Development Build".
    public static bool isDebugBuild { get { return UnityEngine.Debug.isDebugBuild; } }

    //
    // Summary:
    //     Assert the condition.
    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition)
    {
        UnityEngine.Debug.Assert(condition);
    }
    //
    // Summary:
    //     Assert the condition.
    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, string message)
    {
        UnityEngine.Debug.Assert(condition, message);
    }
    //
    // Summary:
    //     Assert the condition.
    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, format, args);
    }

    [Conditional("EDITOR_ONLY")]
    public static void Break()
    {
        UnityEngine.Debug.Break();
    }

    [Conditional("EDITOR_ONLY")]
    public static void ClearDeveloperConsole()
    {
        UnityEngine.Debug.ClearDeveloperConsole();
    }

    [Conditional("EDITOR_ONLY")]
    public static void DebugBreak()
    {
        UnityEngine.Debug.DebugBreak();
    }
    //
    // Summary:
    //     Draws a line between specified start and end points.
    [Conditional("EDITOR_ONLY")]
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        UnityEngine.Debug.DrawLine(start, end);
    }
    //
    // Summary:
    //     Draws a line between specified start and end points.
    [Conditional("EDITOR_ONLY")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        UnityEngine.Debug.DrawLine(start, end, color);
    }
    //
    // Summary:
    //     Draws a line between specified start and end points.
    [Conditional("EDITOR_ONLY")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration);
    }
    //
    // Summary:
    //     Draws a line between specified start and end points.
    [Conditional("EDITOR_ONLY")]
    public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }
    //
    // Summary:
    //     Draws a line from start to start + dir in world coordinates.
    [Conditional("EDITOR_ONLY")]
    public static void DrawRay(Vector3 start, Vector3 dir)
    {
        UnityEngine.Debug.DrawRay(start, dir);
    }
    //
    // Summary:
    //     Draws a line from start to start + dir in world coordinates.
    [Conditional("EDITOR_ONLY")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        UnityEngine.Debug.DrawRay(start, dir, color);
    }
    //
    // Summary:
    //     Draws a line from start to start + dir in world coordinates.
    [Conditional("EDITOR_ONLY")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration);
    }
    //
    // Summary:
    //     Draws a line from start to start + dir in world coordinates.
    [Conditional("EDITOR_ONLY")]
    public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    //
    // Summary:
    //     Logs message to the Unity Console.
    [Conditional("EDITOR_ONLY")]
    public static void Log(object message, UnityEngine.Object context = null)
    {
        UnityEngine.Debug.Log(message, context);
    }

    //
    // Summary:
    //     Logs a formatted error message to the Unity console.
    [Conditional("EDITOR_ONLY")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }
    //
    // Summary:
    //     Logs a formatted error message to the Unity console.
    [Conditional("EDITOR_ONLY")]
    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(context, format, args);
    }
    //
    // Summary:
    //     A variant of Debug.Log that logs an error message to the console.
    [Conditional("EDITOR_ONLY")]
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }
    //
    // Summary:
    //     A variant of Debug.Log that logs an error message to the console.
    [Conditional("EDITOR_ONLY")]
    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }
    //
    // Summary:
    //     Logs a formatted message to the Unity Console.
    [Conditional("EDITOR_ONLY")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }
    //
    // Summary:
    //     Logs a formatted message to the Unity Console.
    [Conditional("EDITOR_ONLY")]
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }
    //
    // Summary:
    //     A variant of Debug.Log that logs a warning message to the console.
    [Conditional("EDITOR_ONLY")]
    public static void LogWarning(object message, UnityEngine.Object context = null)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    //
    // Summary:
    //     Logs a formatted warning message to the Unity Console.
    [Conditional("EDITOR_ONLY")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }
    //
    // Summary:
    //     Logs a formatted warning message to the Unity Console.
    [Conditional("EDITOR_ONLY")]
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }
#endregion

#region New Debug

    [Conditional("EDITOR_ONLY")]
    public static void LogInfo(object message, UnityEngine.Object context = null)
    {
        UnityEngine.Debug.Log(message, context);
    }
    [Conditional("EDITOR_ONLY")]
    public static void LogInfoCat(params object[] args)
    {
        UnityEngine.Debug.Log(string.Concat(args));
    }

    //
    // Summary:
    //     A variant of Debug.Log that logs a warning message to the console.
    [Conditional("EDITOR_ONLY")]
    public static void LogWarningCat(params object[] args)
    {
        UnityEngine.Debug.LogWarning(string.Concat(args));
    }

    //
    // Summary:
    //     Logs message to the Unity Console.
    [Conditional("EDITOR_ONLY")]
    public static void LogCat(params object[] args)
    {
        UnityEngine.Debug.Log(string.Concat(args));
    }
    //
    // Summary:
    //     A variant of Debug.Log that logs an error message to the console.
    [Conditional("EDITOR_ONLY")]
    public static void LogError(object message, UnityEngine.Object context = null)
    {
        UnityEngine.Debug.LogError(message, context);
    }
    //
    // Summary:
    //     A variant of Debug.Log that logs an error message to the console.
    [Conditional("EDITOR_ONLY")]
    public static void LogErrorCat(params object[] args)
    {
        UnityEngine.Debug.LogError(string.Concat(args));
    }

#endregion
}
#endif

namespace Stom
{
    public static class CustomDebug
    {
        public const string define_nameDebugMode    = "DEBUG_MODE";
        public const string define_nameRemoveDebug  = "REPLACE_LOG";

        //
        // Summary:
        //     A variant of Debug.Log that logs a cyan color to the console.
        [Conditional("DEBUG_MODE")]
        public static void LogInfo(object tag = null, object message = null) { UnityEngine.Debug.Log("<color=cyan>(Info) " + tag + ": </color>" + message); }

        //
        // Summary:
        //     Easier and mor quickly to concat string to log to the console.
        [Conditional("DEBUG_MODE")]
        public static void LogDebug(params object[] args) { UnityEngine.Debug.Log("Debug:----------" + string.Concat(args)); }

        //
        // Summary:
        //     A variant of Debug.Log that logs a orange color to the console.
        [Conditional("DEBUG_MODE")]
        public static void LogFocus(object tag = null, object message = null) { UnityEngine.Debug.Log("<color=orange>(Focus) " + tag + ": </color>" + message); }

        //
        // Summary:
        //     A variant of Debug.Log that logs a red color to the console.
        [Conditional("DEBUG_MODE")]
        public static void LogHightlight(object tag = null, object message = null) { UnityEngine.Debug.Log("<color=red>(Hightlight) " + tag + ": </color>" + message); }
    }
}