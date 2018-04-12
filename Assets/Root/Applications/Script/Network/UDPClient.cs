using HoloToolkit.Unity;
using System;
using System.Text;
using UnityEngine;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#else

using System.Net;
using System.Net.Sockets;

#endif

public class UDPClient : Singleton<UDPClient>
{
    [Tooltip("The IPv4 Address of the machine running the Unity editor.")]
    public string ServerIP;

    //[Tooltip("The connection port on the machine to use.")]
    public int ConnectionPort = 11001;

#if !UNITY_EDITOR

    /// <summary>
    /// Send the given data to the server using the established connection
    public async void SendString(string message)
    {
        DatagramSocket socket = new DatagramSocket();
        await socket.ConnectAsync(new HostName(ServerIP), ConnectionPort.ToString());

        using (var writer = new Windows.Storage.Streams.DataWriter(socket.OutputStream))
        {
            var data = Encoding.UTF8.GetBytes(message);

            //writer.WriteBytes(data);
            writer.WriteString(message);
            await writer.StoreAsync();
            //Debug.Log("Sent: " + message);
        }
    }

#else

    /// <summary>
    /// Send the given data to the server using the established connection
    /// </summary>
    /// <param name="serverName">The name of the server</param>
    /// <param name="portNumber">The number of the port over which to send the data</param>
    /// <param name="data">The data to send to the server</param>
    /// <returns>The result of the Send request</returns>
    public void SendString(string message)
    {
        try
        {
            IPAddress ipAddress = IPAddress.Parse(ServerIP);

            // Convert the string data to byte data using ASCII encoding.
            IPEndPoint ep = new IPEndPoint(ipAddress, ConnectionPort);

            byte[] sendBytes = Encoding.UTF8.GetBytes(message);

            //Debug.Log(message);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                    ProtocolType.Udp);
            s.SendTo(sendBytes, ep);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

#endif
}