using UnityEngine;
using UnityEngine.UI;

public class ServerColorIndicator : MonoBehaviour
{
    [SerializeField]
    private SimpleServer server;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Color activeColor;

    [SerializeField]
    private Color deactiveColor;

    private void Awake()
    {
        server.OnRunning += ServerStateChanged;
    }

    private void ServerStateChanged(bool state)
    {
        if (state)
            image.color = activeColor;
        else
            image.color = deactiveColor;
    }
}
