using TMPro;
using UnityEngine;

public class ConsoleView : MonoBehaviour
{
    [SerializeField]
    private Transform container;

    [SerializeField]
    private TMP_Text textLinePrefab;

    private void Awake()
    {
        ConsoleLogger.LogEntryReviced += AddLine;
    }

    private void OnApplicationQuit()
    {
        ConsoleLogger.LogEntryReviced -= AddLine;
    }

    private void AddLine(ConsoleLogger.LogEntry logEntry)
    {
        var line = Instantiate(textLinePrefab, container);
        line.SetText(logEntry.Message);
    }
}
