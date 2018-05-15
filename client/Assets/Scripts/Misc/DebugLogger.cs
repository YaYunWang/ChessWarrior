using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class DebugLogger
{

    [Conditional("ENABLE_LOGS")]
    public static void Log(object message)
    {
        UnityEngine.Debug.unityLogger.Log(LogType.Log, message);   
    }

    [Conditional("ENABLE_LOGS")]
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.unityLogger.Log(LogType.Log, message,context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.unityLogger.Log(LogType.Error, message);  
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.unityLogger.Log(LogType.Error, message,context);  
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogFormat(string format,params object[] args)
    {
        UnityEngine.Debug.unityLogger.LogFormat(LogType.Log, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.unityLogger.LogFormat(LogType.Log, context, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.unityLogger.Log(LogType.Warning, message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.unityLogger.Log(LogType.Warning, message, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.unityLogger.LogException(exception);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.unityLogger.LogException(exception, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.unityLogger.LogFormat(LogType.Error, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.unityLogger.LogFormat(LogType.Error, context, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.unityLogger.LogFormat(LogType.Warning, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.unityLogger.LogFormat(LogType.Warning, context, format, args);
    }

}
