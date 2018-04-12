using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#if UNITY_EDITOR

using System.Net;
using System.Net.Sockets;

#endif

public class UDPServer : MonoBehaviour
{
    [Tooltip("The IPv4 Address of the machine running the Unity editor.")]
    public string ServerIP;

    [Tooltip("The connection port on the machine to use.")]
    public int ConnectionPort = 11001;

#if UNITY_EDITOR

    /// <summary>
    /// Listens for network connections
    /// </summary>
    private UdpClient networkListener;

    private IPEndPoint groupEP;

    private Queue<string> msgQueue = new Queue<string>();

    // Use this for initialization.
    protected virtual void Start()
    {
        listenThread = new Thread(new ThreadStart(SimplestReceiver));
        listenThread.Start();
        return;
    }

    private Thread listenThread;
    private UdpClient listenClient;

    private void SimplestReceiver()
    {
        //Debug.Log(",,,,,,,,,,,, Overall listener thread started.");

        //listenClient = new UdpClient(listenEndPoint);
        listenClient = new UdpClient(ConnectionPort);

        //Debug.Log(",,,,,,,,,,,, listen client started.");

        while (true)
        {
            //Debug.Log(",,,,, listen client listening");

            try
            {
                IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] data = listenClient.Receive(ref listenEndPoint);
                string message = Encoding.UTF8.GetString(data);
                msgQueue.Enqueue(message);
                //Debug.Log("Listener heard: " + message);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode != 10060)
                    Debug.Log("a more serious error " + ex.ErrorCode);
                else
                    Debug.Log("expected timeout error");
            }

            Thread.Sleep(10); // tune for your situation, can usually be omitted
        }
    }

    private void OnDestroy()
    { CleanUp(); }

    private void OnDisable()
    { CleanUp(); }

    // be certain to catch ALL possibilities of exit in your environment,
    // or else the thread will typically live on beyond the app quitting.

    private void CleanUp()
    {
        Debug.Log("Cleanup for listener...");

        // note, consider carefully that it may not be running
        if (listenClient != null)
            listenClient.Close();
        Debug.Log(",,,,, listen client correctly stopped");

        if (listenThread != null)
        {
            listenThread.Abort();
            listenThread.Join(5000);
            listenThread = null;
        }
        Debug.Log(",,,,, listener thread correctly stopped");
    }

    //protected void OnDestroy()
    //{
    //    if (networkListener != null)
    //        networkListener.Close();
    //}

    // Update is called once per frame.
    protected virtual void Update()
    {
        int count = msgQueue.Count;
        while (count-- > 0)
        {
            string msg = msgQueue.Dequeue();
            processMessage(msg);
        }
    }

    protected virtual void processMessage(string msg)
    {
        Debug.Log("Processing " + msg);
    }

    /// <summary>
    /// Called when a client connects.
    /// </summary>
    /// <param name="result">The result of the connection.</param>
    protected void ReceiveCallback(IAsyncResult result)
    {
        Byte[] receiveBytes = networkListener.EndReceive(result, ref groupEP);
        string receiveString = Encoding.ASCII.GetString(receiveBytes);
        msgQueue.Enqueue(receiveString);
        // Request the network listener to wait for connections asynchronously.
        AsyncCallback callback = new AsyncCallback(ReceiveCallback);
        networkListener.BeginReceive(callback, this);
    }

#endif
}