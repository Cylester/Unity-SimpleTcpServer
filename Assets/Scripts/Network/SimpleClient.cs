using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;

public class SimpleClient : MonoBehaviour
{
    [SerializeField]
    private string ipAddress;

    [SerializeField]
    private int port;

    public Action<bool> OnConnected;

    private TcpClient tcpClient;

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Connect()
    {
        tcpClient = new(ipAddress, port);

        OnConnected?.Invoke(true);
    }

    public void Disconnect()
    {
        tcpClient?.Close();
        OnConnected?.Invoke(false);
    }

    public void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);

            NetworkStream stream = tcpClient.GetStream();
            stream.Write(data, 0, data.Length);

            Debug.Log($"Sent: {message}");

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead == 0)
            {
                Debug.Log("Server has closed the connection");
                stream.Close();
                return;
            }

            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log($"Recived: {response}");

            stream.Close();
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }
    }

    public void SendMessage(TMP_InputField inputField)
    {
        SendMessage(inputField.text);
    }
}
