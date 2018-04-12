using HoloToolkit.Unity;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking;
using Windows.Foundation;
#else

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#endif

public class TCPClient : Singleton<TCPClient>
{
    [Tooltip("The IPv4 Address of the machine running the Unity editor.")]
    public string ServerIP;

    [Tooltip("The connection port on the machine to use.")]
    public int ConnectionPort = 11000;

#if !UNITY_EDITOR
    /// <summary>
    /// Tracks the network connection to the remote machine we are sending meshes to.
    /// </summary>
    private StreamSocket networkConnection;
#else
    // ManualResetEvent instances signal completion.

    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);

    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);

#endif

    /// <summary>
    /// Tracks if we are currently sending a mesh.
    /// </summary>
    private bool Sending = false;

    /// <summary>
    /// Temporary buffer for the data we are sending.
    /// </summary>
    private byte[] nextDataBufferToSend;

    /// <summary>
    /// A queue of data buffers to send.
    /// </summary>
    private Queue<byte[]> dataQueue = new Queue<byte[]>();

    /// <summary>
    /// If we cannot connect to the server, we will wait before trying to reconnect.
    /// </summary>
    private float deferTime = 0.0f;

    /// <summary>
    /// If we cannot connect to the server, this is how long we will wait before retrying.
    /// </summary>
    private float timeToDeferFailedConnections = 10.0f;

    public void Update()
    {
        // Check to see if deferTime has been set.
        // DeferUpdates will set the Sending flag to true for
        // deferTime seconds.
        if (deferTime > 0.0f)
        {
            DeferUpdates(deferTime);
            deferTime = 0.0f;
        }

        // If we aren't sending a mesh, but we have a mesh to send, send it.
        if (!Sending && dataQueue.Count > 0)
        {
            byte[] nextPacket = dataQueue.Dequeue();
            Debug.Log("Trying to Send");
            SendDataOverNetwork(nextPacket);
        }
    }

    /// <summary>
    /// Handles waiting for some amount of time before trying to reconnect.
    /// </summary>
    /// <param name="timeout">Time in seconds to wait.</param>
    private void DeferUpdates(float timeout)
    {
        Sending = true;
        Invoke("EnableUpdates", timeout);
    }

    /// <summary>
    /// Stops waiting to reconnect.
    /// </summary>
    private void EnableUpdates()
    {
        Sending = false;
    }

    /// <summary>
    /// Queues up a data buffer to send over the network.
    /// </summary>
    /// <param name="dataBufferToSend">The data buffer to send.</param>
    public void SendString(string msg)
    {
        byte[] dataBufferToSend = Encoding.ASCII.GetBytes(msg);
        dataQueue.Enqueue(dataBufferToSend);
        Debug.Log("Queued packet to Send");
    }

    /// <summary>
    /// Sends the data over the network.
    /// </summary>
    /// <param name="dataBufferToSend">The data buffer to send.</param>
    private void SendDataOverNetwork(byte[] dataBufferToSend)
    {
        if (Sending)
        {
            // This shouldn't happen, but just in case.
            Debug.Log("one at a time please");
            return;
        }

        // Track that we are sending a data buffer.
        Sending = true;

        // Set the next buffer to send when the connection is made.
        nextDataBufferToSend = dataBufferToSend;

        // Setup a connection to the server.
#if !UNITY_EDITOR
        HostName networkHost = new HostName(ServerIP.Trim());
        networkConnection = new StreamSocket();
        // Connections are asynchronous.
        // !!! NOTE These do not arrive on the main Unity Thread. Most Unity operations will throw in the callback !!!
        IAsyncAction outstandingAction = networkConnection.ConnectAsync(networkHost, ConnectionPort.ToString());
        AsyncActionCompletedHandler aach = new AsyncActionCompletedHandler(NetworkConnectedHandler);
        outstandingAction.Completed = aach;
#else
        try
        {
            IPAddress ipAddress = IPAddress.Parse(ServerIP);

            // Convert the string data to byte data using ASCII encoding.

            TcpClient client = new TcpClient();

            client.BeginConnect(ipAddress, ConnectionPort,
                new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne(100);

            NetworkStream netStream = client.GetStream();
            // Write how much data we are sending.
            netStream.WriteByte((byte)dataBufferToSend.Length);

            // Then write the data.
            netStream.Write(dataBufferToSend, 0, dataBufferToSend.Length);

            Sending = false;
            // Always disconnect here since we will reconnect when sending the next message
            client.Close();
        }
        catch (Exception e)
        {
            Sending = false;
            Debug.Log(e.ToString());
        }

#endif
    }

#if !UNITY_EDITOR
    /// <summary>
    /// Called when a connection attempt complete, successfully or not.
    /// !!! NOTE These do not arrive on the main Unity Thread. Most Unity operations will throw in the callback !!!
    /// </summary>
    /// <param name="asyncInfo">Data about the async operation.</param>
    /// <param name="status">The status of the operation.</param>
    public void NetworkConnectedHandler(IAsyncAction asyncInfo, AsyncStatus status)
    {
        // Status completed is successful.
        if (status == AsyncStatus.Completed)
        {
            DataWriter networkDataWriter;

            // Since we are connected, we can send the data we set aside when establishing the connection.
            using (networkDataWriter = new DataWriter(networkConnection.OutputStream))
            {
                // Write how much data we are sending.
                networkDataWriter.WriteInt32(nextDataBufferToSend.Length);

                // Then write the data.
                networkDataWriter.WriteBytes(nextDataBufferToSend);

                // Again, this is an async operation, so we'll set a callback.
                DataWriterStoreOperation dswo = networkDataWriter.StoreAsync();
                dswo.Completed = new AsyncOperationCompletedHandler<uint>(DataSentHandler);
            }
        }
        else
        {
            //drop packet
            return;
            Debug.Log("Failed to establish connection. Error Code: " + asyncInfo.ErrorCode);
            // In the failure case we'll requeue the data and wait before trying again.
            networkConnection.Dispose();

            // Didn't send, so requeue the data.
            dataQueue.Enqueue(nextDataBufferToSend);

            // And set the defer time so the update loop can do the 'Unity things'
            // on the main Unity thread.
            deferTime = timeToDeferFailedConnections;
        }
    }

    /// <summary>
    /// Called when sending data has completed.
    /// !!! NOTE These do not arrive on the main Unity Thread. Most Unity operations will throw in the callback !!!
    /// </summary>
    /// <param name="operation">The operation in flight.</param>
    /// <param name="status">The status of the operation.</param>
    public void DataSentHandler(IAsyncOperation<uint> operation, AsyncStatus status)
    {
        // If we failed, requeue the data and set the deferral time.
        if (status == AsyncStatus.Error)
        {
            // didn't send, so requeue
            dataQueue.Enqueue(nextDataBufferToSend);
            deferTime = timeToDeferFailedConnections;
        }
        else
        {
            // If we succeeded, clear the sending flag so we can send another mesh
            Sending = false;
        }

        // Always disconnect here since we will reconnect when sending the next mesh.
        networkConnection.Dispose();
    }
#else

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            //Socket client = (Socket)ar.AsyncState;

            //// Complete sending the data to the remote device.
            //int bytesSent = client.EndSend(ar);
            //Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            //// Signal that all bytes have been sent.
            //sendDone.Set();
            //Sending = false;
            //// Always disconnect here since we will reconnect when sending the next message
            //client.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

#endif
}