using UnityEngine;
using System.Collections;

public class UDPRedirect : UDPServer
{
    public UDPClient client;

    protected override void processMessage(string msg)
    {
        base.processMessage(msg);

        client.SendString(msg);
    }
}