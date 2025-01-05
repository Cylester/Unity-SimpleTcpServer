using System;
using System.Threading.Tasks;
using UnityEngine;

public class DebugConsoleWatcher : MonoBehaviour
{
    private void Awake()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    private void OnApplicationQuit()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    private void HandleLog(string message, string stackTrace, LogType logType)
    {
        Task.Run(() => ConsoleLogger.LogAsync(message));
    }
}
