using UnityEngine;
using UnityEngine.UI;

public class ClientColorIndicator : MonoBehaviour
{
    [SerializeField]
    private SimpleClient client;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Color activeColor;

    [SerializeField]
    private Color deactiveColor;

    private void Awake()
    {
        client.OnConnected += ClientStateChanged;
    }

    private void ClientStateChanged(bool state)
    {
        if (state)
            image.color = activeColor;
        else
            image.color = deactiveColor;
    }
}
