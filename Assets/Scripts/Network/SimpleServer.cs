using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SimpleServer : MonoBehaviour
{
    private TcpListener tcpListener;
    private List<TcpClient> tcpClients = new();
    private Thread listenerThread;
    private volatile bool isRunning;

    public Action<bool> OnRunning;

    private void OnApplicationQuit()
    {
        StopServer();
    }

    public void StartServer()
    {
        StopServer();

        isRunning = true; 
        listenerThread = new Thread(ListenForClients)
        {
            IsBackground = true
        };
        listenerThread.Start();
        OnRunning?.Invoke(true);
    }

    public void StopServer()
    {
        if (!isRunning)
            return;

        isRunning = false;

        foreach (TcpClient client in tcpClients)
            client.Close();

        tcpClients.Clear();

        tcpListener.Stop(); 
        listenerThread.Join();
        OnRunning?.Invoke(false);
        Debug.Log("Server stopped");
    }

    private void ListenForClients()
    {
        tcpListener = new TcpListener(IPAddress.Any, 8888);
        tcpListener.Start();
        Debug.Log("Server started on port 8888");

        while (isRunning) 
        {
            if (tcpListener.Pending())
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                tcpClients.Add(tcpClient);

                Debug.Log($"[{tcpClient.Client}] Client joined");

                Thread clientThread = new(HandleClientComm)
                {
                    IsBackground = true
                };
                clientThread.Start(tcpClient);
            }
            else
            {
                Thread.Sleep(2000); 
            }
        }
    }

    private void HandleClientComm(object clientObj)
    {
        TcpClient tcpClient = (TcpClient)clientObj;
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = new byte[4096];

        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log($"Recived message: {message}");

            byte[] response = Encoding.ASCII.GetBytes("Message received");
            stream.Write(response, 0, response.Length);
        }

        tcpClient.Close();
    }
}
