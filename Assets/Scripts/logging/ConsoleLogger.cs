using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleLogger
{
    public static List<LogEntry> logEntries = new();

    public static Action<LogEntry> LogEntryReviced;

    public static async UniTask LogAsync(string message)
    {
        var entry = PrepareEntry(message, LogType.Log);

        logEntries.Add(entry);
        await UniTask.SwitchToMainThread();
        LogEntryReviced?.Invoke(entry);
    }

    private static LogEntry PrepareEntry(string message, LogType logType)
    {
        var time = DateTime.Now;
        var entry = new LogEntry(message, time, logType);
        return entry;
    }

    public struct LogEntry
    {
        public DateTime TimeSpan;
        public string Message;
        public LogType Type;

        public LogEntry(string message, DateTime timeSpan,LogType logType)
        {
            Message = message;
            Type = logType;
            TimeSpan = timeSpan;
        }
    } 
}
